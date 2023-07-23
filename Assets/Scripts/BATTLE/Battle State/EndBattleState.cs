using System;
using UnityEngine;
public class EndBattleState : BattleState
{
    public static event Action OnBattleEnd;

    public override void OnEnterState(BattleStateManager battle)
    {
        //PERFORM ACTION AT END OF THE BATTLE
        OnBattleEnd?.Invoke();

        //PLAY OUT REWARD SCREEN AND STUFF
    }
}
