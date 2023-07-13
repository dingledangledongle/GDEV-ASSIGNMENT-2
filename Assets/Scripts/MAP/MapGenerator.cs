using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapGenerator : MonoBehaviour
{
    public int depthCount = 15;
    public int minNodePerDepth = 3;
    public int maxNodePerDepth = 5;
    public float spacing = 2f;
    public int offSetRange = 4;
    public float maxDistanceForConnection = 2f;

    private Graph graph;

    public GameObject nodePrefab;
    public GameObject linePrefab;
    public RandSeedManager randSeed;

    private bool hasDanglingNodes = false;


    //VISUAL
    private int edgeCount = 0;
    //...
    public void GenerateGraph()
    {
        
        graph = new Graph();
        DestroyMap();   //destroying the current map if one exists
        randSeed.GenerateSeed();    //randomising the seed
        //creating the randomized amount of nodes for each depth
        for (int depth = 0; depth < depthCount; depth++)
        {   
            //turn this part into weighted chances
            int nodesPerDepth = Random.Range(minNodePerDepth, maxNodePerDepth);

            for (int i = 0; i < nodesPerDepth; i++)
            {
                float x = i * spacing + 1;
                float y = depth * spacing;

                int nodeId = graph.NodeCount;
                int offsetX = Random.Range(-offSetRange / 2, offSetRange);

                Vector3 position = new(x + offsetX, y, 0);
                //Vector3 position = new(x, y, 0); //change back ltr
                Node node = new();
                node.Id = nodeId;
                node.Depth = depth;
                node.Position = position;

                graph.AddNode(node);
            }
        }


        //adding the edges of the nodes
        for (int depth = 0; depth < depthCount; depth++) //run through each depth
        {
            List<Node> sourceNodes = graph.GetNodesInDepth(depth); //getting the nodes at the current depth
            List<Node> targetNodes = graph.GetNodesInDepth(depth + 1); //getting nodes at the next depth

            
            foreach (Node source in sourceNodes) // loop through the list of nodes at current depth
            {
                //check for nodes that are within distance to be connected

                foreach (Node target in targetNodes)// loop through list of nodes at next depth
                {
                    float distance = Vector3.Distance(source.Position, target.Position); //getting the distance between the nodes

                    if (distance < maxDistanceForConnection)
                    {

                        //if (Random.Range(0, 4) != 0)
                        ///{
                            graph.AddEdge(source.Id, target); //adds an edge if within distance 

                            //VISUAL
                            graph.AddToEdgeList(edgeCount,source);
                            graph.AddToEdgeList(edgeCount, target);
                            edgeCount++;
                            //...
                        //}
                        
                    }
                    
                }
            }
        }
        AddMasterNode();
        DisplayGraph();

        PruneGraph();
        while (hasDanglingNodes)
        {
            PruneGraph();
        }
        if(graph.NodeList.Count < 35)
        {
            GenerateGraph();
        }

    }
    private void AddMasterNode()
    {
        float x = spacing;
        float y = depthCount * spacing;

        int nodeId = graph.NodeCount;
        Vector3 position = new(x, y, 0);
        Node node = new();
        node.Id = nodeId;
        node.Depth = depthCount;
        node.Position = position;

        graph.AddNode(node);
        List<Node> precedingNodes = graph.GetNodesInDepth(depthCount - 1);
        foreach(Node precedingNode in precedingNodes)
        {
            graph.AddEdge(precedingNode.Id, node);
        }

        
    }
    
    private void DisplayGraph()
    {
        edgeCount = 0;
        foreach (Node node in graph.NodeList)
        {
            DisplayNodes(node);
            DisplayLines(node);
        }
    }

    private void DisplayNodes(Node node)
    {
        GameObject nodeObject = Instantiate(nodePrefab, node.Position, Quaternion.identity);    //spawn an instance of a node
        nodeObject.transform.SetParent(GameObject.FindGameObjectWithTag("Graph").transform, false); //setting the parent as "Graph"
        nodeObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = node.Id.ToString();
        nodeObject.name = "Node" + node.Id.ToString();
    }
    private void DisplayLines(Node source)
    {
        List<Node> connectedNodes = graph.GetConnected(source.Id);

        foreach (Node node in connectedNodes)
        {
            Vector3 linePosition = new Vector3(0, 0, 1);    //start point of the edge
            GameObject lineObject = Instantiate(linePrefab, linePosition, Quaternion.identity); //spawn an instance of the edge
            Vector3[] linePath = { source.Position, node.Position };    //start point and end point of the edge
            lineObject.GetComponent<LineRenderer>().SetPositions(linePath); //setting the start and end
            lineObject.transform.SetParent(GameObject.FindGameObjectWithTag("Graph").transform, false); //setting the parent as "Graph"

            //VISUAL
            lineObject.name = "Edge"+edgeCount;
            edgeCount++;
            //...
        }

    }

    private void DestroyMap()
    {
        GameObject[] tilesArray = GameObject.FindGameObjectsWithTag("Node");    //get list of objects in the map
        foreach (var item in tilesArray)
        {
            Destroy(item);
        }

        //VISUAL
        edgeCount = 0;
        //...
    }

    private void PruneGraph()
    {
        List<Node> nodesToRemove = new List<Node>();
        Dictionary<int, List<Node>> AdjacencyList = graph.AdjacencyList;
        GameObject[] listObject = GameObject.FindGameObjectsWithTag("Node");
        
        //VISUAL
        List<int> edgesRemove = new List<int>();
        //...

        //creating a list of nodes to remove
        foreach (Node node in graph.NodeList)
        {
            bool Connected = CheckIfNodeIsConnected(node);
            if (!Connected) // if not connected to any nodes => add to the list of nodes to remove
            {
                nodesToRemove.Add(node);
            }
        }

        //removing the nodes and edges
        foreach (Node node in nodesToRemove)
        {
            //VISUALS ON WHAT IS BEING REMOVED
            foreach (var item in graph.EdgeList)
            {
                if (item.Value.Contains(node.Id)){
                    edgesRemove.Add(item.Key);
                }
            }
           
            string nodeName = "Node" + node.Id.ToString();
            foreach (GameObject obj in listObject)
            {
                if(nodeName == obj.name)
                {
                    obj.GetComponent<SpriteRenderer>().color = Color.red;
                }
               
                foreach (int id in edgesRemove)
                {
                    string edgeName = "Edge" + id.ToString();
                    if(edgeName == obj.name)
                    {
                        obj.GetComponent<LineRenderer>().startColor = Color.red;
                        obj.GetComponent<LineRenderer>().endColor = Color.red;
                    }
                }
                
            }
            //...

            graph.RemoveNode(node);
            hasDanglingNodes = CheckRemainingNodes();
        }
    }

    private bool CheckIfNodeIsConnected(Node node)
    {
        List<Node> connectedNodes = graph.GetConnected(node.Id);

        List<Node> nodesInPrevDepth = graph.GetNodesInDepth(node.Depth - 1);
        bool connected = false;
        foreach (Node nodeInDepth in nodesInPrevDepth) //check if the node is connected to any node from the previous depth
        {
            if (graph.AdjacencyList[nodeInDepth.Id].Contains(node))
            {
                connected = true;
            }            
        }
        if ((!connected && node.Depth != 0))
        {
            return false;
        }else if(connectedNodes.Count < 1 && node.Depth != depthCount - 1)
        {
            if(node.Depth == depthCount)
            {
                return true;
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckRemainingNodes()  //check if there are any remainingnodes
    {
        foreach (Node node in graph.NodeList)
        {
            bool Connected = CheckIfNodeIsConnected(node);
            if (!Connected)
            {
                return true;
            }
        }
        return false;
    }
}
