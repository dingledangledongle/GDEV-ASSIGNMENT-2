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
        // sets the buttonPressed boolean to true in order for the coroutine to continue
        buttonPressed = true;

        // only starts the coroutine once when the player presses the button for the first time
        if (firstPress)
        {
            StartCoroutine(ReachIn());
        }
    }

    public override void Option_2()
    {
        //ends the event
        eventManager.TriggerEvent(Event.RAND_EVENT_END);
    }

    private bool Pressed()
    {
        return buttonPressed;
    }

    private IEnumerator ReachIn()
    {
        firstPress = false; // ensures that this coroutine wouldnt be ran another time if the player presses the button

        while (!rewardReceived) // loop keeps going as long as the player hasn't received a reward
        {
            // get the randomised result from the ProbabilityManager
            string outcome = GetResult();

            if (outcome == "reward")
            {
                //upgrade the player's damage as a reward and break out of the loop to end the event
                eventManager.TriggerEvent(Event.RAND_EVENT_UPGRADEATTACK, 2);
                break;
            }

            //player takes damage each time the button is clicked
            float dmg = 3f;
            eventManager.TriggerEvent<float>(Event.RAND_EVENT_TAKEDAMAGE,3f);

            // makes the amount of damage player take increase by 2 each time they click the button
            dmg += 2f;

            //probability for the reward increases the more the player presses
            rewardsProbability["reward"] += 0.25f;
            rewardsProbability["damage"] -= 0.25f;

            //update text to reflect the new probability
            transform.Find("OptionGroup/Option1/OptionText")
                .GetComponent<TMP_Text>().text = $"[Reach In] Lose {dmg} HP.  {rewardsProbability["reward"] * 100}% Chance to get reward";

            // waits for the button to be pressed before continuing
            buttonPressed = false; 
            yield return new WaitUntil(Pressed); 
        }

        //update text to show player that they have gotten the reward
        transform.Find("EventBody").GetComponent<TMP_Text>().text = "You finally pulled out a ring! \n" +
                    "You feel a surge of power throughout your body.";

        //destroys the option 1 button so player can't press it anymore since they gotten the reward
        Destroy(transform.Find("OptionGroup/Option1").gameObject);
    }

    private string GetResult()
    {
        //runs the dictionary of probability into the ProbabilityManager to get a random result according
        //to the weight of the choices
        string outcome = ProbabilityManager.SelectWeightedItem(rewardsProbability);

        return outcome;
    }
}
