using System;
using UnityEngine;
public class EndEnemyState : BattleState
{
    public override void OnEnterState(BattleStateManager battle)
    {
        battle.UpdateTurnNumber();
        Debug.Log("enemy end");
        //Move to player's turn
        battle.SwitchState(battle.StartPlayerState);

    }
}
