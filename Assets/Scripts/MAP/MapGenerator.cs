using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int depthCount = 15;
    private int minNodePerDepth = 3;
    private int maxNodePerDepth = 5;
    private float spacing = 150f;
    private int offSetRangeX = 75;
    private int offSetRangeY = 15;

    private float maxDistanceForConnection = 220f;
    private float screenOffSetY = -1150;
    private float screenOffSetX = -200;
    private bool hasDanglingNodes = false;


    //Dictionary of the weighted probability of each encounter, it is to be passed into the SelectWeightedItem() method to get a random encounter
    private Dictionary<Node.Encounter, float> encounterProbability = new Dictionary<Node.Encounter, float>() {
        {Node.Encounter.ELITE,1f},
        {Node.Encounter.ENEMY,3f},
        {Node.Encounter.EVENT,2f},
        {Node.Encounter.REST,2f}
    };

    private Graph graph;
    private ProbabilityManager probability;

    public GameObject nodePrefab;
    public GameObject linePrefab;
    public RandSeedManager randSeed;

    //VISUAL
    private int edgeCount = 0;
    //...

    /* This method is called at the moment when the player presses start game.
     * Firstly, it destroyed any map that is existing and it also generates a seed for the random number generator
     *
     * Then, it goes through a for-loop whereby each loop is a depth on the graph.
     * It places a random amount of nodes in each depth from minNodePerDepth to maxNodePerDepth and also determine the properties of the node
     * in the inner for-loop.
     * 
     * After adding the nodes, it will run through another for-loop to place down the edges connecting nearby nodes on the next depth.
     * There is a 1/4 chance of it not placing an edge even though it meets the distance requirement in order to trim down the amount of edges.
     * 
     * It then adds a Master Node at the end which is the boss encounter for the level.
     * 
     * From here, it will run through the graph to check if there is any outlying nodes which does not connect to the next or previous depth and it will proceed
     * to remove the node and its corresponding edges.
     * It will then repeat this process until there are no more outlying nodes in the graph.
     * 
     * If it does not meet the minimum requirement of number of nodes in the graph, it will generate the graph again and repeat the process.
     * 
     * After all the requirements are met, it will display the graph for the player to interact with.
     */
    public void GenerateGraph()
    {
        graph = new Graph();
        probability = new();
        DestroyMap();   //destroying the current map if one exists
        randSeed.GenerateSeed();    //randomising the seed

        //creating the randomized amount of nodes for each depth
        for (int depth = 0; depth < depthCount; depth++)
        {
            int nodesPerDepth = Random.Range(minNodePerDepth, maxNodePerDepth);

            for (int i = 0; i < nodesPerDepth; i++)
            {
                //determining the place where the node will be placed
                float x = screenOffSetX + i * spacing + 1;
                float y = screenOffSetY+ depth * spacing;

                int nodeId = graph.NodeCount;
                int offsetX = Random.Range(-offSetRangeX/2, offSetRangeX);
                int offsetY = Random.Range(-offSetRangeY, offSetRangeY);


                Vector3 position = new(x + offsetX, y +offsetY, 0);
                Node node = new();
                node.Id = nodeId;
                node.Depth = depth;
                node.Position = position;

                /* This places fixed encounter at depth [0 ,14 , half of the map]
                 * If the node is not located in any of those depth, it will get a random encounter instead
                 */
                if (node.Depth == 0)
                {
                    node.EncounterType = Node.Encounter.ENEMY;
                }
                else if (node.Depth == 14)
                {
                    node.EncounterType = Node.Encounter.REST;
                }
                else if (node.Depth == depthCount / 2)
                {
                    node.EncounterType = Node.Encounter.CHEST;
                }
                else
                {
                    GetRandomEncounter(node);
                }

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
                        if (Random.Range(0, 4) != 0)
                        {
                            graph.AddEdge(source.Id, target); //adds an edge if within distance 

                            //VISUAL
                            graph.AddToEdgeList(edgeCount, source);
                            graph.AddToEdgeList(edgeCount, target);
                            edgeCount++;
                            //...
                        }
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
        if (graph.NodeList.Count < 35)
        {
            //GenerateGraph();
        }
    }

    /*
     *  TYPE COMMENT HERE
     */
    private void GetRandomEncounter(Node node)
    {
        //GET WEIGHTED PROBABILITY
        if (node.Depth < 7)
        {
            bool isElite = true;
            while (isElite)
            {
                node.EncounterType = ProbabilityManager.SelectWeightedItem(encounterProbability);
                if (node.EncounterType != Node.Encounter.ELITE)
                {
                    isElite = false;
                }
            }
        }
        else
        {
            node.EncounterType = ProbabilityManager.SelectWeightedItem(encounterProbability);
        }
    }
    /*
     *  TYPE COMMENT HERE
     */
    private void AddMasterNode()
    {
        List<Node> precedingNodes = graph.GetNodesInDepth(depthCount - 1);
        float x = 0;
        float y = precedingNodes[0].Position.y + spacing;

        int nodeId = graph.NodeCount;
        Vector3 position = new(x, y, 0);
        Node node = new();
        node.Id = nodeId;
        node.Depth = depthCount;
        node.Position = position;
        node.EncounterType = Node.Encounter.BOSS;

        graph.AddNode(node);
        foreach (Node precedingNode in precedingNodes)
        {
            graph.AddEdge(precedingNode.Id, node);
        }
    }

    /*
     *  TYPE COMMENT HERE
     */
    private void DisplayGraph()
    {
        edgeCount = 0;
        foreach (Node node in graph.NodeList)
        {
            DisplayNodes(node);
            DisplayLines(node);
        }
    }

    /*
     *  TYPE COMMENT HERE
     */
    private void DisplayNodes(Node node)
    {
        GameObject nodeGameObject = Instantiate(nodePrefab, node.Position, Quaternion.identity);    //spawn an instance of a node
        nodeGameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Graph").transform, false); //setting the parent as "Graph"
        nodeGameObject.name = "Node" + node.Id.ToString();

        NodeObject nodeObject = nodeGameObject.GetComponent<NodeObject>();
        nodeObject.Node = node;
        //nodeObject.SetSprite();
    }

    /*
     *  TYPE COMMENT HERE
     */
    private void DisplayLines(Node source)
    {
        List<Node> connectedNodes = graph.GetConnected(source.Id);

        foreach (Node node in connectedNodes)
        {
            Vector3 linePosition = new Vector3(0, 0, 20);    //start point of the edge
            GameObject lineObject = Instantiate(linePrefab, linePosition, Quaternion.identity); //spawn an instance of the edge
            Vector3[] linePath = { source.Position, node.Position };    //start point and end point of the edge
            lineObject.GetComponent<LineRenderer>().SetPositions(linePath); //setting the start and end
            lineObject.transform.SetParent(GameObject.FindGameObjectWithTag("Graph").transform, false); //setting the parent as "Graph"

            //VISUAL
            lineObject.name = "Edge" + edgeCount;
            edgeCount++;
            //...
        }

    }

    /*
     *  TYPE COMMENT HERE
     */
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

    /*
     *  TYPE COMMENT HERE
     */
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
                if (item.Value.Contains(node.Id))
                {
                    edgesRemove.Add(item.Key);
                }
            }

            string nodeName = "Node" + node.Id.ToString();
            foreach (GameObject obj in listObject)
            {
                if (nodeName == obj.name)
                {
                    obj.GetComponent<SpriteRenderer>().color = Color.red;
                }

                foreach (int id in edgesRemove)
                {
                    string edgeName = "Edge" + id.ToString();
                    if (edgeName == obj.name)
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

    /*
     *  This method check the specific node that was passed into it
     *  if it has any edges connecting it to the nodes in the next depth or previous depth
     *  
     *  If there are either no connection to the next or previous depth, it will return false
     */
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
        }
        else if (connectedNodes.Count < 1 && node.Depth != depthCount - 1)
        {
            if (node.Depth == depthCount)
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

    /*
     *  Checks through the entire list of nodes if any of them is not connected, returning true if any
     *  of the node is not connected
     */
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
