using UnityEngine;

public class MapImporter : MonoBehaviour
{
    [SerializeField] public GameObject NodePrefab;
    OSMParser OsmParser;

    private void Start()
    {
        OsmParser = new OSMParser();
        OsmParser.LoadOSM("D:\\Games\\osm-unity-graph\\Assets\\Data\\map.osm");

        foreach (var node in OsmParser.nodes.Values)
        {
            Vector2 position = node.position;
            GameObject nodeObject = Instantiate(NodePrefab, position, Quaternion.identity);
            nodeObject.name = $"Node_{node.id}";
        }

        foreach (var way in OsmParser.ways)
        {
            for (int i = 0; i < way.nodeRefs.Count - 1; i++)
            {
                if (OsmParser.nodes.TryGetValue(way.nodeRefs[i], out var startNode) &&
                    OsmParser.nodes.TryGetValue(way.nodeRefs[i + 1], out var endNode))
                {
                    CreateEdge(startNode.position, endNode.position);
                }
            }
        }
    }

    private void CreateEdge(Vector2 start, Vector2 end)
    {
        GameObject line = new GameObject("Line");
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
