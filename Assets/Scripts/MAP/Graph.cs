using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public Dictionary<int, List<Node>> AdjacencyList;
    public List<Node> NodeList = new List<Node>();
    
    public int NodeCount = 0;




    //VISUAL
    public int EdgeCount = 0;
    public Dictionary<int, List<int>> EdgeList;
    //...
    public Graph()
    {
        AdjacencyList = new Dictionary<int, List<Node>>();

        //VISUAL
        EdgeList = new Dictionary<int, List<int>>();
        //...
    }

    public void AddNode(Node node) //adding the node
    {
        NodeList.Add(node);
        AdjacencyList[node.Id] = new List<Node>();
        EdgeList[node.Id] = new List<int>();
        NodeCount++;

    }

    public void AddEdge(int sourceId, Node target)
    {
        if(AdjacencyList.ContainsKey(sourceId) && AdjacencyList.ContainsKey(target.Id)) //check if the adjacency list contains the source and the target
        {
            AdjacencyList[sourceId].Add(target);
            EdgeList[EdgeCount] = new List<int>();
            EdgeCount++;

        }
    }

    //VISUAL
    public void AddToEdgeList(int edgeid,Node target)
    {
        EdgeList[edgeid].Add(target.Id);
        
    }
    //...

    public void RemoveNode(Node node)
    {
        //removing instances of the removed node from the edge list
        foreach (var item in AdjacencyList)
        {
            List<Node> connectedNodes = item.Value;
            if (connectedNodes.Contains(node))
            {
                connectedNodes.Remove(node);
            }
        }
        //removing the node from the adjacency list
        AdjacencyList.Remove(node.Id);
        NodeList.Remove(node);

        //NO IMPLEMENTATION OF REMOVING EDGES FROM EDGELIST YET

    }

    public List<Node> GetConnected(int id)
    {
        if (AdjacencyList.ContainsKey(id))//searches the list and returns a list of nodes connected
        {
            return AdjacencyList[id];
        }

        return new List<Node>();
    }

    //VISUAL
    public List<int> GetNodeEdges(int id)
    {
        if (EdgeList.ContainsKey(id))
        {
            return EdgeList[id];
        }

        return new List<int>();
    }
    //...

    public List<Node> GetNodesInDepth(int depth)
    {
        List<Node> NodesInDepth = new List<Node>();

        foreach(Node node in NodeList) //looping the list of nodes
        {
            if(node.Depth == depth)//checking depth
            {
                NodesInDepth.Add(node);
            }
        }

        return NodesInDepth;
    }
}
