using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public Dictionary<int, List<Node>> AdjacencyList;
    public List<Node> NodeList = new List<Node>();
    public int NodeCount = 0;

    public Graph()
    {
        AdjacencyList = new Dictionary<int, List<Node>>();
    }

    public void AddNode(Node node)
    {
        NodeList.Add(node);
        AdjacencyList[node.Id] = new List<Node>();
        NodeCount++;
    }

    public void AddEdge(int sourceId, Node target)
    {
        if(AdjacencyList.ContainsKey(sourceId) && AdjacencyList.ContainsKey(target.Id))
        {
            AdjacencyList[sourceId].Add(target);
        }
    }

    public List<Node> GetConnected(int id)
    {
        if (AdjacencyList.ContainsKey(id))
        {
            return AdjacencyList[id];
        }

        return new List<Node>();
    }

    public List<Node> GetNodesInDepth(int depth)
    {
        List<Node> NodesInDepth = new List<Node>();

        foreach(Node node in NodeList)
        {
            if(node.Depth == depth)
            {
                NodesInDepth.Add(node);
            }
        }

        return NodesInDepth;
    }
}
