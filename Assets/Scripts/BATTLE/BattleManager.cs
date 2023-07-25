using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    private HUDHandler hudHandler;
    private Player player;
    private List<Enemy> enemyList;

    private void Start()
    {
        hudHandler = new();
        enemyList = new();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        foreach (GameObject enemyObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyList.Add(enemyObject.GetComponent<Enemy>());
        }

        AttackAction.OnAttackSuccess += UpdateHud;
        DefendAction.OnDefend += UpdateHud;
        MaterialAction.OnAfterEnhance += UpdateHud;


        StartPlayerState.OnDisplayReady += UpdateHud;
        StartEnemyState.OnEnemyStart += GetEnemyList;
        StartEnemyState.OnEnemyAction += UpdateHud;

        Player.OnPlayerDamageTaken += UpdateHud;
        setupBattle();
    }

 
    private List<Enemy> GetEnemyList()
    {
        return enemyList;
    }
    private void setupBattle()
    {
        //START FIRST TURN STUFF
        UpdateHud();
    }

    private void UpdateHud()
    {
        hudHandler.SetHUD(player, enemyList);
    }
}
