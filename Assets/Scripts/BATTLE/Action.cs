using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public GameObject ArrowPrefab;
    private GameObject arrowObject;
    // Update is called once per frame
    private void OnMouseDown()
    {
        Vector3 mousePosition = Input.mousePosition;
        
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Debug.Log("Attack");
        arrowObject = Instantiate(ArrowPrefab, spawnPosition, Quaternion.identity);
        arrowObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        arrowObject.transform.localScale = new Vector3(1, 1, 1);
    }

    private void Update()
    {
        if(arrowObject != null)
        {
            /*Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            Vector3 arrowPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            arrowObject.transform.position = arrowPosition;
            Debug.Log(Input.mousePosition.x +" , "+ Input.mousePosition.y);*/
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;
        if(collidedObject.CompareTag("Enemy"))
        {
            Debug.Log("st");
        }
    }


}
