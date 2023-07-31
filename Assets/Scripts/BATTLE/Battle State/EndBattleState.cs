using System;
using System.Collections;
using UnityEngine;
public class EndBattleState : BattleState
{
    public static event Action OnBattleEnd; //EncounterManager.EndBattle()

    public override void OnEnterState(BattleStateManager battle)
    {
        //PERFORM ACTION AT END OF THE BATTLE
        battle.StartCoroutine(WaitForAllUpdates());

        Debug.Log("end battle");

        //PLAY OUT REWARD SCREEN AND STUFF
    }

    private IEnumerator WaitForAllUpdates()
    {
        yield return new WaitForSeconds(1f);
        OnBattleEnd?.Invoke();
    }
}
