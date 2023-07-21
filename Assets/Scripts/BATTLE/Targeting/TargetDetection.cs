using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    GameObject target;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            target = collision.gameObject;
        }
    }
    private void OnMouseUp()
    {
        if(target != null)
        {
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            target = null;
        }
    }
}

    