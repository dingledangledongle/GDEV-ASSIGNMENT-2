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
        //referencing components
        wheelBody = transform.Find("Circle").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //constantly reduces the rotation of the wheel
        ReduceSpeed();


        if(wheelBody.angularVelocity <= 0) 
        {
            // once the wheel comes to a stop, it will get the result from ResultDetection class
            result = eventManager.TriggerEvent<GameObject>(Event.RAND_EVENT_STW_WHEELSTOP);
            hasStopped = true;
        }
    }

    public void Spin()
    {
        //this method adds a random amount of torque to the wheel to spin it

        float randTorque = Random.Range(500, 1000);
        wheelBody.AddTorque(randTorque,ForceMode2D.Impulse);
    }

    private void ReduceSpeed()
    {
        // adds more angular drag as long as the wheel is still spinning
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
