using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager : MonoBehaviour
{
    BattleState currentState;
    StartBattleState StartBattleState = new();
    StartPlayerState StartPlayerState = new();
    EndPlayerState EndPlayerState = new();
    StartEnemyState StartEnemyState = new();
    EndEnemyState EndEnemyState = new();
    EndBattleState EndBattleState = new();

    public void SwitchState(BattleState state)
    {
        currentState = state;
        state.EnterState(this);
    } 
}
