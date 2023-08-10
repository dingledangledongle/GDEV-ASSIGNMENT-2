using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTheWheel : RandomEvent
{
    private RandomEvents eventName = RandomEvents.SpinTheWheel;
    private float amtToUpgrade = 1f;
    private DamageType damage = new(15f,1);

    private EventManager eventManager = EventManager.Instance;
    public GameObject wheelPrefab;
    private SpinWheel spinWheelScript;
    private GameObject spinWheelObject;
    private string result;
    
    public override RandomEvents EventName {
        get => eventName; 
    }

    public override void Option_1() {
        Transform parentTransform = transform.parent.parent;
        
        //spawns the wheel for the event
        spinWheelObject = Instantiate(wheelPrefab, parentTransform);
        spinWheelScript = spinWheelObject.GetComponent<SpinWheel>();

        //start spinning the wheel
        spinWheelScript.Spin();

        //wait for the wheel to stop and then proceed with the rest of the coroutine
        StartCoroutine(WaitForSpin());
    }

    public override void Option_2() {
        //leave the event by ending it
        eventManager.TriggerEvent(Event.RAND_EVENT_END);
    }

    private IEnumerator WaitForSpin()
    {
        yield return new WaitUntil(spinWheelScript.HasStopped); // wait for the wheel to stop before continuing
        result = spinWheelScript.ReturnResult();
        ApplyEffect(result); // applies the effect according to the received result

        // add a delay so the player can see where the wheel stop at before ending the event
        yield return new WaitForSeconds(1); 
        Destroy(spinWheelObject); 
        eventManager.TriggerEvent(Event.RAND_EVENT_END);
    }

    private void ApplyEffect(string resultName)
    {
        switch (resultName)
        {
            case "UpgradeDefense":
                eventManager.TriggerEvent<float>(Event.RAND_EVENT_UPGRADEDEFEND,amtToUpgrade);
                break;
            case "Heal":
                eventManager.TriggerEvent<float>(Event.RAND_EVENT_HEAL, 15f);
                break;
            case "Shanked":
                eventManager.TriggerEvent<DamageType>(Event.RAND_EVENT_TAKEDAMAGE, damage);      
                break;
            case "UpgradeAttack":
                eventManager.TriggerEvent<float>(Event.RAND_EVENT_UPGRADEATTACK, amtToUpgrade);
                break;
            case "UpgradeHealth":
                eventManager.TriggerEvent<float>(Event.RAND_EVENT_UPGRADEHEALTH, 15f);
                break;
            case "DecreaseHealth":
                eventManager.TriggerEvent<float>(Event.RAND_EVENT_REDUCEMAXHEALTH, -15f);
                break;
        }
    }
}
