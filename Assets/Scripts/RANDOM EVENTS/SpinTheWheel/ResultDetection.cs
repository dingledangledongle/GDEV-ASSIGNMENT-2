using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDetection : MonoBehaviour
{
    private GameObject result;
    private void Start()
    {
        EventManager.Instance.AddListenerWithReturn<GameObject>(Event.RAND_EVENT_STW_WHEELSTOP,ReturnResult);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        result = collision.gameObject;
    }

    private GameObject ReturnResult()
    {
        return result;
    }
}
