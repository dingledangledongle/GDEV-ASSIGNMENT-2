using System;
using System.Collections;
using UnityEngine;
public class StartPlayerState : BattleState
{
    private EventManager eventManager = EventManager.Instance;
    public override void OnEnterState(BattleStateManager battle)
    {
   
        //PERFORM ACTIONS AT START OF PLAYER'S TURN
        
        Debug.Log("player turn start");
        
        battle.StartCoroutine(StartPlayerTurn());
    }

    private IEnumerator StartPlayerTurn()
    {
        eventManager.TriggerEvent(Event.PLAYER_TURN);
        yield return new WaitForSeconds(1f);
        eventManager.TriggerEvent(Event.PLAYER_ROLLDICE);
        eventManager.TriggerEvent(Event.UPDATE_HUD);
        
        yield return new WaitUntil(IsDiceStationary);
        eventManager.TriggerEvent(Event.PLAYER_DICE_FINISHED);
    }

    private bool IsDiceStationary()
    {
        bool stationary = eventManager.TriggerEvent<bool>(Event.PLAYER_ROLLDICE);
        return stationary;
    }
}
