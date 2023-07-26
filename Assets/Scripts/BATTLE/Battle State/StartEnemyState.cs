using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class StartEnemyState : BattleState
{
    public static event Action OnEnterEnemyStart;
    public static event Action OnEnemyStart;
    public static event Action OnEnemyAction;
    public static event Func<bool> OnEnemyFinishAction;
    
    public override void OnEnterState(BattleStateManager battle)
    {
        Debug.Log("enemy start");

        //PERFORM ACTION AT START OF ENEMY TURN
        battle.StartCoroutine(ExecuteActions(battle));
    }

    private IEnumerator ExecuteActions(BattleStateManager battle)
    {
        OnEnterEnemyStart.Invoke();
        OnEnemyStart?.Invoke();
        OnEnemyAction?.Invoke();
        yield return new WaitUntil(OnEnemyFinishAction);
        //MOVE TO END TURN
        battle.SwitchState(battle.EndEnemyState);
    }

}
