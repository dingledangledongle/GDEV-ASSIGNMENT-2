using System;
using UnityEngine;
public class EndPlayerState : BattleState
{
    public static event Action OnPlayerEnd;
    public override void OnEnterState(BattleStateManager battle)
    {
        //PERFORM ACTIONS AT END OF PLAYER TURN
        OnPlayerEnd?.Invoke();

        //Move to enemy start
        battle.SwitchState(battle.StartEnemyState);
    }
}
