using System.Xml.Linq;
using System.IO;
using System.Linq;
using UnityEngine;

public class OSMParser
{
    public void LoadOSM(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found");
        }
        
        XDocument doc = XDocument.Load(filePath);

        foreach (var node in doc.Descendants("node"))
        {
            string id = node.Attribute("id")?.Value;
            double lat = double.Parse(node.Attribute("lat")?.Value);
            double lon = double.Parse(node.Attribute("lon")?.Value);
        }
        
        foreach (var way in doc.Descendants("way"))
        {
            string id = way.Attribute("id")?.Value;
            var ndRefs = way.Descendants("nd").Select(nd => nd.Attribute("ref")?.Value).ToList();

            foreach (var tag in way.Descendants("tag"))
            {
                string key = tag.Attribute("k")?.Value;
                string value = tag.Attribute("v")?.Value;
            }
        }
    }
}
