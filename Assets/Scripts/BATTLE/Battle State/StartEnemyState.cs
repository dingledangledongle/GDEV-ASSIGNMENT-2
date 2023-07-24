using System;
using System.Collections.Generic;
using UnityEngine;
public class StartEnemyState : BattleState
{
    public static event Func<List<Enemy>> OnEnemyStart;
    
    List<Enemy> enemyList;
    public override void OnEnterState(BattleStateManager battle)
    {
        Debug.Log("enemy start");

        //PERFORM ACTION AT START OF ENEMY TURN
        enemyList = OnEnemyStart?.Invoke();

        foreach (Enemy enemy in enemyList)
        {
            enemy.PerformAction();
        }
        //MOVE TO END TURN
        battle.SwitchState(battle.EndEnemyState);
    }
}
