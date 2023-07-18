using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeObject : MonoBehaviour
{
    public Node Node { get; set; }
    public Sprite[] spriteArray;
    public SpriteRenderer spriteRender;

    private bool circleUpdate = false;
    private float circleSpeed = 0.015f;
    private void OnMouseDown()
    {
        //HEHEHAHA
        if(Node != null)
        {
            Debug.Log("ID : " + Node.Id + " Encounter : " + Node.EncounterType + " DEPTH : " + Node.Depth);
            
            circleUpdate = true;
        }
    }

    private void Update()
    {
        if (circleUpdate)
        {
            CircleNode();
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

    private void CircleNode()
    {
        Image circle = this.GetComponentInChildren<Image>();

        if (circle.fillAmount == 1.0f)
        {
            circleUpdate = false;
        }
        circle.fillAmount += circleSpeed;
    }

}
