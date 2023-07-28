using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefendAction : MonoBehaviour, IPointerDownHandler
{
    private int energyCost = 1;
    public AudioSource deniedSFX;
    public AudioSource defendSFX;


    //EVENTS
    public static Action OnDefend; //Player.Defend() , Player.PlayDefendAnim();
    public static event Action OnUpdateHud; //BattleManager.UpdateHud()
    public static event Func<int, bool> BeforeDefend; //Player.IsEnoughEnergy()
    public static event Action<int> OnAfterDef; //Player.ReduceCurrentEnergy()

    public void OnPointerDown(PointerEventData data)
    {
        if (BeforeDefend.Invoke(energyCost))
        {
            OnDefend.Invoke();
            OnAfterDef?.Invoke(energyCost);
            defendSFX.Play();
            OnUpdateHud?.Invoke();

        }
        else
        {
            deniedSFX.Play();
        }
        
    }
}
