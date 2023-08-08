using System;
using System.Collections;
using UnityEngine;
public class EndBattleState : BattleState
{

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
        EventManager.Instance.TriggerEvent(Event.BATTLE_END);
    }
}
