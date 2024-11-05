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
        GUILayout.Label("OSM File Importer", EditorStyles.boldLabel);
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
        parent.transform.position = Vector3.zero;
        if (osmParser == null)
        {
            osmParser = new OSMParser();
        }

        osmParser.LoadOSM(filePath, scale);

        foreach (var node in osmParser.nodes.Values)
        {
            Vector2 position = node.position;
            GameObject nodeObject = Instantiate(nodePrefab, position, Quaternion.identity, parent.transform);
            nodeObject.name = $"Node_{node.id}";
        }

        foreach (var way in osmParser.ways)
        {
            for (int i = 0; i < way.nodeRefs.Count - 1; i++)
            {
                if (osmParser.nodes.TryGetValue(way.nodeRefs[i], out var startNode) &&
                    osmParser.nodes.TryGetValue(way.nodeRefs[i + 1], out var endNode))
                {
                    CreateEdge(startNode.position, endNode.position, parent);
                }
            }
        }
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
