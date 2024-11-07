using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Dictionary<string, Node> nodes = new Dictionary<string, Node>();
    public List<Way> ways = new List<Way>();

    private void Start()
    {
        Debug.Log(nodes.Count);
    }

    public void UpdateNodesAndWays(Dictionary<string, Node> nodes, List<Way> ways)
    {
        this.nodes = nodes;
        this.ways = ways;
    }
}
