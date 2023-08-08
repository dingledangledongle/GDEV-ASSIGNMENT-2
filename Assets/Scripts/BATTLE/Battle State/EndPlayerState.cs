using System;
public class EndPlayerState : BattleState
{
    public override void OnEnterState(BattleStateManager battle)
    {
        //Move to enemy start
        battle.SwitchState(battle.StartEnemyState);
    }
}
