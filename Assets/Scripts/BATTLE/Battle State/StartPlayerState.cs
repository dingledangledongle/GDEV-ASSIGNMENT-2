using System;
using UnityEngine;
using System.Collections;
public class StartPlayerState : BattleState
{
    public static event Action OnPlayerStart; //Subscribers : Enemy.GetIntent()
    public static event Action OnRollDice; // BattleManager.RollDice()
    public static event Action OnDisplayReady; //Subscribers : BattleManager.SetHUD()
    public static event Func<bool> OnDiceFinish; //DiceHandler.IsAllDiceStatonary()
    public static event Action OnMaterialListUpdate;
    public override void OnEnterState(BattleStateManager battle)
    {
   
        //PERFORM ACTIONS AT START OF PLAYER'S TURN
        
        Debug.Log("player turn start");
        
        battle.StartCoroutine(StartPlayerTurn());
    }

    private IEnumerator StartPlayerTurn()
    {
        OnPlayerStart?.Invoke();
        yield return new WaitForSeconds(1f);
        OnRollDice?.Invoke();
        OnDisplayReady?.Invoke();
        yield return new WaitUntil(OnDiceFinish);
        OnMaterialListUpdate?.Invoke();
    }
}
