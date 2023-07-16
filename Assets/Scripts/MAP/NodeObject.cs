using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeObject : MonoBehaviour
{
    public Node Node { get; set; }

    private void OnMouseDown()
    {
        //HEHEHAHA
        if(Node != null)
        {
            Debug.Log("ID : " +Node.Id+" Encounter : "+ Node.EncounterType + " DEPTH : " + Node.Depth);
        }
    }
}
