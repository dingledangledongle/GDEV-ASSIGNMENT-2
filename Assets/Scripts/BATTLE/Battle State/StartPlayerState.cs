using System;
using UnityEngine;
public class StartPlayerState : BattleState
{
    public static event Action OnPlayerStart; //Subscribers : Enemy.GetIntent()
    public static event Action OnDisplayReady; //Subscribers : BattleManager.SetHUD()
    public override void OnEnterState(BattleStateManager battle)
    {
   
        //PERFORM ACTIONS AT START OF PLAYER'S TURN
        OnPlayerStart?.Invoke();
        OnDisplayReady?.Invoke();
        Debug.Log("player turn start");
    }

}
