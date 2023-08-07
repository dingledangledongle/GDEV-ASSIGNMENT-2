using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefendAction : MonoBehaviour, IPointerDownHandler
{
    private int energyCost = 1;
    public AudioSource deniedSFX;
    public AudioSource defendSFX;
    private EventManager eventManager = EventManager.Instance;

    public void OnPointerDown(PointerEventData data)
    {
        bool isEnoughEnergy = eventManager.TriggerEvent<int, bool>(Event.PLAYER_DEFEND, energyCost);
        if (isEnoughEnergy)
        {
            eventManager.TriggerEvent(Event.PLAYER_DEFEND);
            eventManager.TriggerEvent<int>(Event.PLAYER_DEFEND, energyCost);
            defendSFX.Play();
            eventManager.TriggerEvent(Event.PLAYER_DEFEND_FINISHED);

        }
        else
        {
            deniedSFX.Play();
        }
        
    }
}
