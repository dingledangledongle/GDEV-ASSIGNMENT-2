using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    private Enemy target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the arrow hits the enemy's collider, it will enable the target box that was inactive before.
        // It also sets the target variable to the enemy object it collided with.
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<Enemy>().IsTargetable)
        {
            collision.gameObject.transform.Find("Canvas/Target").GetComponent<SpriteRenderer>().enabled = true;
            target = collision.gameObject.GetComponent<Enemy>();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // When the arrow leaves the enemy's collider, it will disable the target box
        // It also sets the target to null
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.transform.Find("Canvas/Target").GetComponent<SpriteRenderer>().enabled = false;
            target = null;
        }
    }

    public bool isTargetSelected() //Checks if there is a target at the point of this function being called
    {
        if (target != null)
        {
            return true;
        }
        return false;
    }

    public Enemy GetTarget() // Returns the target only if there is a target being selected
    {
        if (isTargetSelected())
        {
            return target;
        }
        return null;
    }

}

