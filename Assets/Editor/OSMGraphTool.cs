using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class OSMGraphTool : EditorWindow
{
    private GameObject nodePrefab;
    private string filePath = "";
    private float scale = 5000f;
    private OSMParser osmParser;

    [MenuItem("Tools/OSM Graph Tool")]
    public static void ShowWindow()
    {
        GetWindow<OSMGraphTool>("OSM Graph Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("OSM File Path:");
        EditorGUILayout.BeginHorizontal();
        filePath = EditorGUILayout.TextField(filePath);
        if (GUILayout.Button("Browse", GUILayout.Width(70)))
        {
            string selectedPath = EditorUtility.OpenFilePanel("Select OSM File", "", "osm");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                filePath = selectedPath;
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Node Prefab:");
        nodePrefab = (GameObject) EditorGUILayout.ObjectField(nodePrefab, typeof(GameObject), false);
        GUILayout.Label("Scale:");
        scale = EditorGUILayout.FloatField(scale);

        if (GUILayout.Button("Generate Nodes and Ways"))
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError("File not found at the specified path.");
                return;
            }

            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        GameObject parent = new GameObject("Graph");
        parent.AddComponent<Graph>();
        parent.transform.position = Vector3.zero;
        if (osmParser == null)
        {
            osmParser = new OSMParser();
        }

        osmParser.LoadOSM(filePath, scale);
        parent.GetComponent<Graph>().UpdateNodesAndWays(osmParser.nodes, osmParser.ways);

        if (nodePrefab != null)
        {
            foreach (var node in osmParser.nodes.Values)
            {
                Vector2 position = node.position;
                GameObject nodeObject = Instantiate(nodePrefab, position, Quaternion.identity, parent.transform);
                nodeObject.name = $"Node_{node.id}";
            }
        }

        GameObject lineMeshObject = new GameObject("LineMesh");
        lineMeshObject.transform.SetParent(parent.transform);
        MeshFilter meshFilter = lineMeshObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = lineMeshObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default")) { color = Color.magenta };
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        foreach (var way in osmParser.ways)
        {
            for (int i = 0; i < way.nodeRefs.Count - 1; i++)
            {
                if (osmParser.nodes.TryGetValue(way.nodeRefs[i], out var startNode) &&
                    osmParser.nodes.TryGetValue(way.nodeRefs[i + 1], out var endNode))
                {
                    Vector3 startPosition = new Vector3(startNode.position.x, startNode.position.y, 0) / scale;
                    Vector3 endPosition = new Vector3(endNode.position.x, endNode.position.y, 0) / scale;

                    int startIndex = vertices.Count;
                    vertices.Add(startPosition);
                    vertices.Add(endPosition);

                    indices.Add(startIndex);
                    indices.Add(startIndex + 1);
                }
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }

    private void CreateEdge(Vector2 start, Vector2 end, GameObject parent)
    {
        GameObject line = new GameObject("Line");
        line.transform.parent = parent.transform;
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.useWorldSpace = false;
    }
}
