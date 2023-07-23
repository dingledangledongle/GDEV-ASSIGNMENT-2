using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private Sprite[] spriteArray;
    [SerializeField] private GameObject Highlight;

    public void Init(int randNum)
    {
        spriteRender.sprite = spriteArray[randNum];

    }

    void OnMouseEnter()
    {
        Highlight.SetActive(true);
    }
    void OnMouseExit()
    {
        Highlight.SetActive(false);
    }

}
