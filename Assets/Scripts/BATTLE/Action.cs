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
        Vector3 spawnPosition = this.GetComponent<RectTransform>().position;
        
        if(arrowObject == null)
        {
            arrowObject = Instantiate(ArrowPrefab, spawnPosition, Quaternion.identity);
            arrowObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            arrowObject.transform.localScale = new Vector3(1, 1, 1);
        }   
    }

    private void OnMouseDrag()
    {
        arrowObject.GetComponent<BezierArrow>().GetMousePosition();
    }

    private void OnMouseUp()
    {
        if(arrowObject != null)
        {
            Destroy(arrowObject);
        }
    }


}
