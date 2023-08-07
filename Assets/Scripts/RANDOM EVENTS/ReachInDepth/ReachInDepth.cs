using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ReachInDepth : RandomEvent
{
    private RandomEvents eventName = RandomEvents.ReachInDepth;
    private bool buttonPressed = false;
    private EventManager eventManager = EventManager.Instance;
    private bool firstPress = true;
    private bool rewardReceived = false;

    private Dictionary<string, float> rewardsProbability = new Dictionary<string, float>()
    {
        {"reward",0.25f },
        {"damage",0.75f }
    };

    public override RandomEvents EventName
    {
        get => eventName;
    }

    public override void Option_1()
    {
        buttonPressed = true;
        if (firstPress)
        {
            StartCoroutine(ReachIn());
        }
    }

    public override void Option_2()
    {
        eventManager.TriggerEvent(Event.RAND_EVENT_END);
    }

    private bool Pressed()
    {
        return buttonPressed;
    }

    private IEnumerator ReachIn()
    {
        firstPress = false;
        while (!rewardReceived)
        {
            // do the effect
            string outcome = GetResult();
            if (outcome == "reward")
            {
                eventManager.TriggerEvent(Event.RAND_EVENT_UPGRADEATTACK, 2);
                
                break;
            }
            eventManager.TriggerEvent<float>(Event.RAND_EVENT_TAKEDAMAGE,3f);
            rewardsProbability["reward"] += 0.25f;
            rewardsProbability["damage"] -= 0.25f;
            transform.Find("OptionGroup/Option1/OptionText")
                .GetComponent<TMP_Text>().text = $"[Reach In] Lose 3 HP.  {rewardsProbability["reward"] * 100}% Chance to get reward";
            buttonPressed = false;

            yield return new WaitUntil(Pressed);
        }
        transform.Find("EventBody").GetComponent<TMP_Text>().text = "You finally pulled out a ring! \n" +
                    "You feel a surge of power throughout your body.";
        Destroy(transform.Find("OptionGroup/Option1").gameObject);


    }

    private string GetResult()
    {
        string outcome = ProbabilityManager.SelectWeightedItem(rewardsProbability);

        return outcome;
    }
}
