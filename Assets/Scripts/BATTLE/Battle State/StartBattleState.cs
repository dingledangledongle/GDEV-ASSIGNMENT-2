using System;
using UnityEngine;
public class StartBattleState : BattleState
{
    public static event Action OnBattleStart;
    public override void OnEnterState(BattleStateManager battle)
    {
        //PERFORM ACTION AT START OF BATTLE
        //LOAD RELICS EFFECT
        OnBattleStart?.Invoke();

        Debug.Log("BATTLE START");
        //MOVE TO PLAYER'S TURN
        
        battle.SwitchState(battle.StartPlayerState);
    }
}
