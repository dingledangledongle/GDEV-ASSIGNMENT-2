using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int depthCount = 15;
    public int minNodePerDepth = 1;
    public int maxNodePerDepth = 4;
    public float spacing = 2f;
    public int offSetRange = 4;
    public float maxDistanceForConnection = 4f;

    private Graph graph;

    public GameObject nodePrefab;
    public GameObject linePrefab;
    public RandSeedManager randSeed;

    public void GenerateGraph()
    {
        graph = new Graph();
        randSeed.GenerateSeed();
        DestroyMap();

        //creating the randomized amount of notes for each depth
        for (int depth = 0; depth < depthCount; depth++)
        {   
            //turn this part into weighted chances
            int nodesPerDepth = Random.Range(1, 4);

            for (int i = 0; i < nodesPerDepth; i++)
            {
                float x = i * spacing;
                float y = depth * spacing;

                int nodeId = graph.NodeCount;
                int offsetX = Random.Range(-offSetRange / 2, offSetRange);

                Vector3 position = new(x + offsetX, y, 0);
                Node node = new();
                node.Id = nodeId;
                node.Depth = depth;
                node.Position = position;

                graph.AddNode(node);
            }
        }

        //adding the edges of the nodes
        for (int depth = 0; depth < depthCount; depth++)
        {
            List<Node> sourceNodes = graph.GetNodesInDepth(depth);
            List<Node> targetNodes = graph.GetNodesInDepth(depth + 1);
            foreach (Node source in sourceNodes)
            {
                foreach (Node target in targetNodes)
                {
                    float distance = Vector3.Distance(source.Position, target.Position);

                    if (distance < maxDistanceForConnection)
                    {
                        graph.AddEdge(source.Id, target);
                        DisplayLines(graph, source);
                    }
                    
                }
            }
        }
        DisplayNodes(graph);
    }
    
    private void DisplayNodes(Graph graph)
    {
        foreach (Node node in graph.NodeList)
        {
            GameObject nodeObject = Instantiate(nodePrefab, node.Position, Quaternion.identity);
            nodeObject.transform.SetParent(GameObject.FindGameObjectWithTag("Stuff").transform, false);
        }
    }

    private void DisplayLines(Graph graph,Node source)
    {
        List<Node> connectedNodes = graph.GetConnected(source.Id);
        foreach (Node node in connectedNodes)
        {
            Vector3 linePosition = new Vector3(0, 0, 1);
            GameObject lineObject = Instantiate(linePrefab, linePosition, Quaternion.identity);
            Vector3[] linePath = { source.Position, node.Position };
            lineObject.GetComponent<LineRenderer>().SetPositions(linePath);
            lineObject.transform.SetParent(GameObject.FindGameObjectWithTag("Stuff").transform, false);

        }

    }

    private void DestroyMap()
    {
        GameObject[] tilesArray = GameObject.FindGameObjectsWithTag("Tile");
        foreach (var item in tilesArray)
        {
            Destroy(item);
        }
    }
}
