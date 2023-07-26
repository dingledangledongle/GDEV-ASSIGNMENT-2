using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Collections;

public class NodeObject : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Node Node { get; set; }
    public Sprite[] spriteArray;
    private Image image;
    private Animator animator;

    private bool circleUpdate = false;
    private float circleSpeed = 0.1f;
    private Color disableColor = new Color(0, 0, 0, 0.6f);
    private Color enableColor = new Color(0, 0, 0, 1f);

    public static event Action<Node> OnClick;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Node != null && Node.IsAccesible)
        {
            Debug.Log("ID : " + Node.Id + " Encounter : " + Node.EncounterType + " DEPTH : " + Node.Depth);
            StartCoroutine(CircleNode());
            animator.Play("NoAnim");
            OnClick?.Invoke(Node); //DISABLES OTHER NODES IN THE SAME DEPTH

        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = enableColor;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!Node.IsAccesible)
        {
            image.color = disableColor;

        }
    }


    private void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
        image = this.gameObject.GetComponent<Image>();
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
        image.color = enableColor;
    }
    public void MakeInAccessible()
    {
        Debug.Log("not access" + Node.Id);

        Node.IsAccesible = false;
        animator.Play("NoAnim");
        image.color = disableColor;

    }
    public void SetSprite()
    {
        switch (Node.EncounterType)
        {
            case Node.Encounter.ENEMY:
                image.sprite = spriteArray[0];
                break;
            case Node.Encounter.ELITE:
                image.sprite = spriteArray[1];
                break;
            case Node.Encounter.EVENT:
                image.sprite = spriteArray[2];
                break;
            case Node.Encounter.REST:
                image.sprite = spriteArray[3];
                break;
            case Node.Encounter.CHEST:
                image.sprite = spriteArray[4];
                break;
            case Node.Encounter.BOSS:
                image.sprite = spriteArray[5];
                break;
        }
    }

    private IEnumerator CircleNode()
    {
        Debug.Log("Circling now...");
        Image circle = this.transform.Find("ink-swirl").GetComponent<Image>();

        for (float i = 0; i < 1; i+= circleSpeed)
        {
            circle.fillAmount += circleSpeed;
            yield return new WaitForSeconds(0.02f);

        }
        
    }

    
}
