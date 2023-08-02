using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RandomEventHandler : MonoBehaviour
{
    //event ui elements
    private TMP_Text titleText;
    private TMP_Text bodyText;
    private Image image;
    private TMP_Text optionText1;
    private TMP_Text optionText2;
    public Sprite[] spriteArray;

    private RandomEvents currentEvent;
    private EventManager eventManager = EventManager.Instance;

    private void Awake()
    {
        //assigning the variables
        titleText = transform.Find("Panel/EventTitle").GetComponent<TMP_Text>();
        bodyText = transform.Find("Panel/EventBody").GetComponent<TMP_Text>();
        image = transform.Find("Panel/EventArt").GetComponent<Image>();
        optionText1 = transform.Find("Panel/OptionGroup/Option1/OptionText").GetComponent<TMP_Text>();
        optionText2 = transform.Find("Panel/OptionGroup/Option2/OptionText").GetComponent<TMP_Text>();

        //events
        eventManager.AddListener<Node>(Event.RAND_EVENT_INITIAZLIZE, Initialize);

    }

    private void Initialize(Node node) {
        //get current event
        
        //update hud to event's text
    }

    private void UpdateEventHUD(RandomEvent randEvent)
    {
        //updating text
        titleText.text = randEvent.Title;
        bodyText.text = randEvent.Body;
        optionText1.text = randEvent.Option1_Text;
        optionText2.text = randEvent.Option2_Text;

        //updating image
        image.sprite = GetEventImage(randEvent.EventName);
        
    }

    private Sprite GetEventImage(RandomEvents eventName)
    {
        switch (eventName)
        {
            case RandomEvents.SpinTheWheel:
                return spriteArray[0];
            case RandomEvents.FreeUpgrade:
                return spriteArray[1];

        }
        return default;
    }
}
