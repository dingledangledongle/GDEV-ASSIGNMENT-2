using UnityEngine;

public class MapEvents : MonoBehaviour
{

}
public class NodeAddedEvent
{
    public Node Node { get; set; }
    public NodeAddedEvent(Node node)
    {
        Node = node;
    }
}
public class NodeRemovedEvent
{
    public Node Node { get; set; }
    public NodeRemovedEvent(Node node)
    {
        Node = node;
    }
}
