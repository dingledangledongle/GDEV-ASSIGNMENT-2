using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RandomEventHandler : MonoBehaviour
{
    private RandomEvents currentEventName;
    private EventManager eventManager = EventManager.Instance;
    public GameObject[] eventPrefabs;
    private GameObject currentEventObject;
    private void Awake()
    {
        //events
        eventManager.AddListener<Node>(Event.RAND_EVENT_INITIALIZE, Initialize);
    }

     private void Initialize(Node node) {
        //get current event
        currentEventName = node.RandomEvent;
        GetEvent(currentEventName);


        //update hud to event's text
    }
   
    public void test()
    {
        Instantiate(eventPrefabs[0], this.transform);

    }

    private void GetEvent(RandomEvents eventName)
    {
        switch (eventName)
        {
            case RandomEvents.SpinTheWheel:
                currentEventObject = Instantiate(eventPrefabs[0],this.transform);
                break;
            case RandomEvents.FreeUpgrade:
                break;
        }
    }

    private void EndEvent()
    {
        Destroy(currentEventObject);
    }
    
}
