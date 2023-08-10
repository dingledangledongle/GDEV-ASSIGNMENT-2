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
        EventManager.Instance.AddListener<GameObject>(Event.PLAYER_ENHANCE_MOUSE_RELEASE,GetAction);

        //change the sprite of the prefab that spawned to match the material that was clicked
        SetSprite();
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<GameObject>(Event.PLAYER_ENHANCE_MOUSE_RELEASE, GetAction);
    }

    public void UpdatePosition()
    {
        //makes the spawned icon follow the player's mouse

        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        objectPosition.z = 1;
        this.gameObject.transform.position = objectPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Attack") || collision.gameObject.CompareTag("Defend"))
        {
            collidedObject = collision.gameObject; //update the current action that it has collided with
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
