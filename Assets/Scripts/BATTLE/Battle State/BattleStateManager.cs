using UnityEngine;
using TMPro;

public class BattleStateManager : MonoBehaviour
{
    public BattleState CurrentState;
    public StartBattleState StartBattleState = new();
    public StartPlayerState StartPlayerState = new();
    public EndPlayerState EndPlayerState = new();
    public StartEnemyState StartEnemyState = new();
    public EndEnemyState EndEnemyState = new();
    public EndBattleState EndBattleState = new();
    public int NumberOfTurns = 1;
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

    public void UpdateTurnNumber()
    {
       
        NumberOfTurns += 1;
        Debug.Log(NumberOfTurns);
        GameObject.Find("TurnText").GetComponent<TMP_Text>().text = "TURN "+ NumberOfTurns.ToString();
    }

}
