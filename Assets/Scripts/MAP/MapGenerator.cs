using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapGenerator : MonoBehaviour
{
    #region VARIABLES
    private int depthCount = 15;
    private int minNodePerDepth = 3;
    private int maxNodePerDepth = 5;
    private float spacing = 150f;
    private int offSetRangeX = 75;
    private int offSetRangeY = 15;

    private float maxDistanceForConnection = 220f;
    private float screenOffSetY = -1150;
    private float screenOffSetX = -200;
    private Color disabledColor = new Color(0, 0, 0, 0.6f);

    //Dictionary of the weighted probability of each encounter, it is to be passed into the SelectWeightedItem() method to get a random encounter
    private Dictionary<Node.Encounter, float> encounterProbability = new Dictionary<Node.Encounter, float>() {
        {Node.Encounter.ELITE,1f},
        {Node.Encounter.ENEMY,3f},
        {Node.Encounter.EVENT,2f},
        {Node.Encounter.REST,2f}
    };

    private Graph graph;

    public GameObject nodePrefab;
    public GameObject linePrefab;
    public RandSeedManager randSeed;
    public GameObject[] EnemyList;
    public GameObject[] EliteList;
    public GameObject[] BossList;
    //VISUAL
    private int edgeCount = 0;
    //...
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        NodeObject.OnClick += DisableNodesInDepth ;
        NodeObject.OnClick += ConnectedNodeAccessible;
        GenerateGraph();
    }

    private void OnDestroy()
    {
        NodeObject.OnClick -= DisableNodesInDepth;
        NodeObject.OnClick -= ConnectedNodeAccessible;
    }

    private void GenerateGraph()
    {
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
        graph = new Graph();
        randSeed.GenerateSeed();    //randomising the seed

        GenerateNodes();
        GenerateEdges();

        AddMasterNode();
        while (CheckForDanglingNodes())
        {
            PruneGraph();
        }
        if (graph.NodeList.Count < 35)
        {
            
            GenerateGraph();
        }
        else
        {
            DisplayGraph();
        }
        
    }
    /*
     *  TYPE COMMENT HERE
     */
    private void DisableNodesInDepth(Node node)
    {
        List<Node> nodesInDepth = graph.GetNodesInDepth(node.Depth);
        foreach (Node nodeInDepth in nodesInDepth)
        {
            if(nodeInDepth.Id == node.Id)
            {
                continue;
            }
            string nodeObjName = "Node" + nodeInDepth.Id.ToString();
            GameObject.Find(nodeObjName).GetComponent<NodeObject>().MakeInAccessible();
        }
    }
    private void ConnectedNodeAccessible(Node node) {

        List<Node> nodesConnected = graph.GetConnected(node.Id);
        foreach (Node nodeConnected in nodesConnected)
        {
            string nodeObjName = "Node" + nodeConnected.Id.ToString();
            GameObject.Find(nodeObjName).GetComponent<NodeObject>().MakeAccessible();
        }
    }
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

    private void AssignEnemies(Node node)
    {
        int numOfEnemies = Random.Range(1, 3);
        if(node.EncounterType == Node.Encounter.ENEMY)
        {
            for (int i = 0; i < numOfEnemies; i++)
            {
                int randomIndex = Random.Range(0, EnemyList.Length);
                
                node.AddEnemy(EnemyList[randomIndex]);
            }
        }else if(node.EncounterType == Node.Encounter.ELITE)
        {
            int randomIndex = Random.Range(0, EliteList.Length);
            node.AddEnemy(EliteList[randomIndex]);
        }else if(node.EncounterType == Node.Encounter.BOSS)
        {
            int randomIndex = Random.Range(0, BossList.Length);
            node.AddEnemy(BossList[0]);
        }
        
    }

    private void AddMasterNode()
    {/* This method goes after GenerateNodes().
      * It generates the boss node located at the the highest depth and aligns itself to the middle of the x-axis
     *  It connects all the nodes from the preceding depth to it
     */
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

        Edge edge = new();
        edge.Id = edgeCount;
        edge.Target = node;
        foreach (Node precedingNode in precedingNodes)
        {
            edge.Source = precedingNode;
            graph.AddEdge(precedingNode.Id, node,edgeCount,edge);
        }
    }
    private void GenerateNodes()
    {
        //creating the randomized amount of nodes for each depth
        for (int depth = 0; depth < depthCount; depth++)
        {
            int nodesPerDepth = Random.Range(minNodePerDepth, maxNodePerDepth);

            for (int i = 0; i < nodesPerDepth; i++)
            {
                //determining the place where the node will be placed
                float x = screenOffSetX + i * spacing + 1;
                float y = screenOffSetY + depth * spacing;

                int nodeId = graph.NodeCount;
                int offsetX = Random.Range(-offSetRangeX / 2, offSetRangeX);
                int offsetY = Random.Range(-offSetRangeY, offSetRangeY);

                Vector3 position = new(x + offsetX, y + offsetY, 0);
                Node node = new();
                node.Id = nodeId;
                node.Depth = depth;
                node.Position = position;

                /* This places fixed encounter at depth [0 ,14 , half of the map]
                 * If the node is not located in any of those depth, it will get a random encounter instead
                 * It also makes the nodes at the starting depth accessible so the players can choose their starting path
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
                AssignEnemies(node);
                graph.AddNode(node);
            }
        }
    }
    private void GenerateEdges()
    {
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
                            Edge edge = new();
                            edge.Id = edgeCount;
                            edge.Source = source;
                            edge.Target = target;
                            graph.AddEdge(source.Id, target,edgeCount,edge); //adds an edge if within distance 

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
    }
    private void DisplayGraph()
    {
     /*
     *  Goes through the list of node and display the nodes
     */
        edgeCount = 0;
        foreach (Node node in graph.NodeList)
        {
            DisplayNodes(node);
            DisplayLines(node);
        }
    }
    private void DisplayNodes(Node node)
    {
     /* Spawn a node game object using the node prefab assigned
     *  The position of the node is already part of the Node attribute.
     *  It then assigns that node object the id of the node and set its color to visually indicate
     *  that its inaccessible
     *  It then sets the sprites accordingly to the assigned encounter
     *  Then, at depth 0 , it makes all the node there accessible for the players to have a starting point
     */ 
        GameObject nodeGameObject = Instantiate(nodePrefab, node.Position, Quaternion.identity);    //spawn an instance of a node
        nodeGameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Graph").transform, false); //setting the parent as "Graph"
        nodeGameObject.name = "Node" + node.Id.ToString();
        nodeGameObject.GetComponent<Image>().color = new Color(0,0,0,0.6f);
        NodeObject nodeObject = nodeGameObject.GetComponent<NodeObject>();
        nodeObject.Node = node;
        nodeObject.SetSprite();  
        if(node.Depth == 0)
        {
            nodeObject.MakeAccessible();
        }
    }

    /*
     *  TYPE COMMENT HERE
     */
    private void DisplayLines(Node source)
    {
        List<Node> connectedNodes = graph.GetConnected(source.Id);
        foreach (Node node in connectedNodes)
        {
            Vector3 linePosition = new Vector3(0, 0, 500);    //start point of the edge
            GameObject lineObject = Instantiate(linePrefab, linePosition, Quaternion.identity); //spawn an instance of the edge
            Vector3[] linePath = { source.Position, node.Position };    //start point and end point of the edge
            lineObject.GetComponent<LineRenderer>().SetPositions(linePath); //setting the start and end
            lineObject.transform.SetParent(GameObject.FindGameObjectWithTag("Graph").transform, false); //setting the parent as "Graph"
            lineObject.GetComponent<LineRenderer>().startColor = disabledColor;
            lineObject.GetComponent<LineRenderer>().endColor = disabledColor;

            //VISUAL
            lineObject.name = "Edge" + edgeCount;
            edgeCount++;
            //...
        }

    }

    /*
     *  TYPE COMMENT HERE
     */
    private void DestroyMap() // not exactly working as intended...
    {

        GameObject[] tilesArray = GameObject.FindGameObjectsWithTag("Node"); //get list of objects in the map
        Debug.Log("tilesArray length : " + tilesArray.Length);
        foreach (var item in tilesArray)
        {
            Destroy(item);
            Debug.Log("destroyed " +item.name);
        }
        graph = null;
        Debug.Log("Map destroyed");
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
            DisplayRemovedNodes(node, listObject, edgesRemove);

            graph.RemoveNode(node);
        }
    }

    private bool CheckIfNodeIsConnected(Node node)
    {
        /*
     *  This method check the specific node that was passed into it
     *  if it has any edges connecting it to the nodes in the next depth or previous depth
     *  
     *  If there are either no connection to the next or previous depth, it will return false
     */
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

    private bool CheckForDanglingNodes()  //check if there are any remainingnodes
    {
        /*
     *  Checks through the entire list of nodes if any of them is not connected, returning true if any
     *  of the node is not connected
     */
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
    
    //USED FOR DEBUGGING
    private void DisplayRemovedNodes(Node node, GameObject[] listObject, List<int> edgesRemove)
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
                obj.GetComponent<Image>().color = Color.red;
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
    }
}
