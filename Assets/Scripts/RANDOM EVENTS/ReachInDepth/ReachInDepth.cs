using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachInDepth : RandomEvent
{
    private RandomEvents eventName = RandomEvents.ReachInDepth;
    private bool buttonPressed = false;
    private EventManager eventManager = EventManager.Instance;

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
        
    }

    public override void Option_2()
    {
        throw new System.NotImplementedException();
    }

    private bool Pressed()
    {
        return buttonPressed;
    }
    private IEnumerator ReachIn()
    {
        for (int i = 0; i < 6; i++)
        {
            // do the effect
            yield return new WaitUntil(Pressed);
            buttonPressed = false;
        }
    }

    private void GetChance()
    {
        string outcome = ProbabilityManager.SelectWeightedItem(rewardsProbability);

        if(outcome == "reward")
        {
            eventManager.TriggerEvent<float>(Event.RAND_EVENT_UPGRADEATTACK, 1f);
            return;
        }
    }
}
