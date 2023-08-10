using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWheel : MonoBehaviour
{
    private Rigidbody2D wheelBody;
    private EventManager eventManager = EventManager.Instance;
    private GameObject result;
    private bool hasStopped = false;
    private void Awake()
    {
        wheelBody = transform.Find("Circle").GetComponent<Rigidbody2D>();

        
    }
    private void Update()
    {
        ReduceSpeed();
        if(wheelBody.angularVelocity <= 0)
        {
            result = eventManager.TriggerEvent<GameObject>(Event.RAND_EVENT_STW_WHEELSTOP);
            hasStopped = true;
        }
    }
    public void Spin()
    {
        float randTorque = Random.Range(500, 1000);
        wheelBody.AddTorque(randTorque,ForceMode2D.Impulse);
    }
    private void ReduceSpeed()
    {
        if (wheelBody.angularVelocity > 0)
        {
            wheelBody.angularDrag += 0.001f;
        }
    }
    public bool HasStopped()
    {
        return hasStopped;
    }

    public string ReturnResult()
    {
        return result.name;
    }

}
