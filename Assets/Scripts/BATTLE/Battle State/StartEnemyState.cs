using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class StartEnemyState : BattleState
{
    public static event Action OnEnterEnemyStart; //Enemy.TurnStart()
    public static event Action OnEnemyStart; //BattleManager.OnEnemyStart()
    public static event Action OnEnemyAction; //BattleManager.UpdateHud()
    public static event Func<bool> OnEnemyFinishAction; //BattleManager.IsEnemyDone()

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
        yield return new WaitForSeconds(0.5f);
        //MOVE TO END TURN
        battle.SwitchState(battle.EndEnemyState);
    }

}
