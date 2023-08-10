using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDetection : MonoBehaviour
{
    private GameObject result;
    private void Start()
    {
        EventManager.Instance.AddListener<GameObject>(Event.RAND_EVENT_STW_WHEELSTOP,ReturnResult);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // each section of the wheel is split up by separating gameobject with colliders of different names
        // this sets the result to the corresponding gameobject the pointer is collided with
        result = collision.gameObject;
    }

    private GameObject ReturnResult()
    {
        return result;
    }
}
