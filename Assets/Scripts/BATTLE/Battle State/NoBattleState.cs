using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NoBattleState : BattleState
{
    public static event Action OnNoBattle;
    public override void OnEnterState(BattleStateManager battle)
    {

    }
}
