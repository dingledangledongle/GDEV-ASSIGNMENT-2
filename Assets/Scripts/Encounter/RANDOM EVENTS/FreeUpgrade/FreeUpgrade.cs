using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FreeUpgrade : RandomEvent
{
    private RandomEvents eventName = RandomEvents.FreeUpgrade;
    private EventManager eventManager = EventManager.Instance;
    private TMP_Text bodyText;
    private GameObject option1;

    private void Awake()
    {
        bodyText = transform.Find("EventBody").GetComponent<TMP_Text>();
        option1 = transform.Find("OptionGroup/Option1").gameObject;
    }

    public override RandomEvents EventName
    {
        get => eventName;
    }

    public override void Option_1()
    {
        GetRewards();
    }

    public override void Option_2()
    {
        //end the event
        eventManager.TriggerEvent(Event.RAND_EVENT_END);
    }

    public void GetRewards()
    {
        int randIndex = Random.Range(0,3); // gets a random number so the rewards can be determined later by the switch case
        Debug.Log(randIndex);
        switch (randIndex)
        {
            //trigger the appropriate event for each case and update the text to reflect the upgrade they received
            // removes the option so that they cant receive the reward again

            case 0:
                eventManager.TriggerEvent(Event.RAND_EVENT_UPGRADEATTACK, 1);
                bodyText.text = "You open the chest to find a surge of light surrounding you\n" +
                    "You feel a surge of strength.";
                Destroy(option1);
                break;

            case 1:
                eventManager.TriggerEvent(Event.RAND_EVENT_UPGRADEDEFEND, 1);
                bodyText.text = "You open the chest to find a surge of light surrounding you. \n" +
                    "You feel more sturdy.";
                Destroy(option1);
                break;

            case 2:
                eventManager.TriggerEvent(Event.RAND_EVENT_UPGRADEHEALTH, 15);
                bodyText.text = "You open the chest to find a surge of light surrounding you. \n" +
                    "You feel a surge of vitality.";
                Destroy(option1);
                break;
        }
    }

    
}
