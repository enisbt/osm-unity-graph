using UnityEngine;

public class MapImporter : MonoBehaviour
{
    private OSMParser parser;
    
    private void Start()
    {
        parser = new OSMParser();
        parser.LoadOSM("D:\\Games\\osm-unity-graph\\Assets\\Data\\map.osm");
    }
}
