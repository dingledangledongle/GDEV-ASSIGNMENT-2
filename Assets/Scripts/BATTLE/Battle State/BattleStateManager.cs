using UnityEngine;

public class BattleStateManager : MonoBehaviour
{
    public BattleState CurrentState;
    public StartBattleState StartBattleState = new();
    public StartPlayerState StartPlayerState = new();
    public EndPlayerState EndPlayerState = new();
    public StartEnemyState StartEnemyState = new();
    public EndEnemyState EndEnemyState = new();
    public EndBattleState EndBattleState = new();

    public void SwitchState(BattleState state)
    {
        CurrentState = state;
        state.OnEnterState(this);
    }

    public void PlayerEndTurn()
    {
        if(CurrentState == StartPlayerState)
        {
            SwitchState(EndPlayerState);
        }
    }

    private void Start()
    {
        SwitchState(StartBattleState);

    }

}
