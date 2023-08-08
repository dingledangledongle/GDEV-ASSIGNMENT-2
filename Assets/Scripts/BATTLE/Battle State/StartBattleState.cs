using System;
using UnityEngine;
public class StartBattleState : BattleState
{
    public override void OnEnterState(BattleStateManager battle)
    {
        //PERFORM ACTION AT START OF BATTLE
        Debug.Log("BATTLE START");
        //MOVE TO PLAYER'S TURN
        
        battle.SwitchState(battle.StartPlayerState);
    }
}
