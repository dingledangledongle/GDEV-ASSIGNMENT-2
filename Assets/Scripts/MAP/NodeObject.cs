using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeObject : MonoBehaviour
{
    public Node Node { get; set; }
    public Sprite[] spriteArray;
    public SpriteRenderer spriteRender;
    private void OnMouseDown()
    {
        //HEHEHAHA
        if(Node != null)
        {
            Debug.Log("ID : " + Node.Id + " Encounter : " + Node.EncounterType + " DEPTH : " + Node.Depth);
        }
    }
    public void SetSprite()
    {
        switch (Node.EncounterType)
        {
            case Node.Encounter.ENEMY:
                spriteRender.sprite = spriteArray[0];
                break;
            case Node.Encounter.ELITE:
                spriteRender.sprite = spriteArray[1];
                break;
            case Node.Encounter.EVENT:
                spriteRender.sprite = spriteArray[2];
                break;
            case Node.Encounter.REST:
                spriteRender.sprite = spriteArray[3];
                break;
            case Node.Encounter.CHEST:
                spriteRender.sprite = spriteArray[4];
                break;
            case Node.Encounter.BOSS:
                spriteRender.sprite = spriteArray[5];
                break;
        }
    }

}
