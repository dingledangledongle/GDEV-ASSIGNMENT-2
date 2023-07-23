using UnityEngine;

public class MaterialPrefab : MonoBehaviour
{
    public Sprite[] SpriteArray;
    private GameObject collidedObject;

    private void Start()
    {
        MaterialAction.OnMouseRelease += GetAction;
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

    public GameObject GetAction()
    {
        if (collidedObject != null)
        {
            return collidedObject.gameObject;
        }
        return null;
    }
}
