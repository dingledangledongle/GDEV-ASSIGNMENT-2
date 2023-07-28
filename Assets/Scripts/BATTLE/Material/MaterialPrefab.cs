using UnityEngine;
using UnityEngine.UI;

public class MaterialPrefab : MonoBehaviour
{
    public Sprite[] spriteArray;
    private GameObject collidedObject;
    private Image image;
    private void Start()
    {
        image = this.GetComponent<Image>();
        MaterialAction.OnMouseRelease += GetAction;
        SetSprite();
    }

    private void OnDestroy()
    {
        MaterialAction.OnMouseRelease -= GetAction;

    }
    public void UpdatePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        objectPosition.z = 1;
        this.gameObject.transform.position = objectPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Attack") || collision.gameObject.CompareTag("Defend"))
        {
            collidedObject = collision.gameObject;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collidedObject != null)
        {
            collidedObject = null;
        }
    }

    private GameObject GetAction()
    {
        if (collidedObject != null)
        {
            return collidedObject.gameObject;
        }
        return null;
    }

    private void SetSprite()
    {
        switch (this.name)
        {
            case "Cloth":
                image.sprite = spriteArray[0];
                break;
            case "Twine":
                image.sprite = spriteArray[1];
                break;
            case "Leather":
                image.sprite = spriteArray[2];
                break;
            case "Wood":
                image.sprite = spriteArray[3];
                break;
            case "Stone":
                image.sprite = spriteArray[4];
                break;
            case "Metal":
                image.sprite = spriteArray[5];
                break;
            case "Glass":
                image.sprite = spriteArray[6];
                break;
            case "EleCrystal":
                image.sprite = spriteArray[7];
                break;
        }
    }
}
