using UnityEngine;
using UnityEngine.UI;
using System;

public class NodeObject : MonoBehaviour
{
    public Node Node { get; set; }
    public Sprite[] spriteArray;
    public SpriteRenderer spriteRender;
    private Animator animator;

    private bool circleUpdate = false;
    private float circleSpeed = 0.015f;
    private Color disableColor = new Color(0, 0, 0, 0.6f);
    private Color enableColor = new Color(0, 0, 0, 1f);

    public static event Action<Node> OnClick;
    private void OnMouseDown()
    {   
        if (Node != null && Node.IsAccesible)
        {
            //Debug.Log("ID : " + Node.Id + " Encounter : " + Node.EncounterType + " DEPTH : " + Node.Depth);
            circleUpdate = true;
            animator.Play("NoAnim");
            OnClick?.Invoke(Node); //DISABLES OTHER NODES IN THE SAME DEPTH

        }
    }

    private void OnMouseEnter()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = enableColor;
    }
    private void OnMouseExit()
    {
        if (!Node.IsAccesible) {
            this.gameObject.GetComponent<SpriteRenderer>().color = disableColor;

        }
    }
    private void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if (circleUpdate)
        {
            CircleNode();
        }
    }
    public void MakeAccessible()
    {
        Node.IsAccesible = true;
        animator.Play("NodeAnimation");
        this.gameObject.GetComponent<SpriteRenderer>().color = enableColor;
    }
    public void MakeInAccessible()
    {
        Debug.Log("not access" + Node.Id);

        Node.IsAccesible = false;
        animator.Play("NoAnim");
        this.gameObject.GetComponent<SpriteRenderer>().color = disableColor;

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
