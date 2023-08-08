using System;
using System.Collections;
using UnityEngine;

public class StartEnemyState : BattleState
{
    private EventManager eventManager = EventManager.Instance;

    public override void OnEnterState(BattleStateManager battle)
    {
        Debug.Log("enemy start");

        //PERFORM ACTION AT START OF ENEMY TURN
        battle.StartCoroutine(ExecuteActions(battle));
    }

    private IEnumerator ExecuteActions(BattleStateManager battle)
    {
        eventManager.TriggerEvent(Event.ENEMY_TURN);

        yield return new WaitUntil(IsDone);
        yield return new WaitForSeconds(0.5f);
        //MOVE TO END TURN
        battle.SwitchState(battle.EndEnemyState);
    }

    private bool IsDone()
    {
        return eventManager.TriggerEvent<bool>(Event.ENEMY_TURN);
    }
}
