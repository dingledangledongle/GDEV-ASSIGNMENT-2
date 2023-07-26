using System;
using UnityEngine;
public class EndEnemyState : BattleState
{
    public static event Action OnEnemyEnd;

    public override void OnEnterState(BattleStateManager battle)
    {
        //PERFORM ACTION AT END OF ENEMY TURN
        OnEnemyEnd?.Invoke();
        battle.UpdateTurnNumber();
        Debug.Log("enemy end");
        //Move to player's turn
        battle.SwitchState(battle.StartPlayerState);

    }
}
