using System;
using UnityEngine;
public class StartEnemyState : BattleState
{
    public static event Action OnEnemyStart;
    public override void OnEnterState(BattleStateManager battle)
    {
        //PERFORM ACTION AT START OF ENEMY TURN
        OnEnemyStart?.Invoke();

        Debug.Log("enemy start");
        //MOVE TO END TURN
        battle.SwitchState(battle.EndEnemyState);
    }
}
