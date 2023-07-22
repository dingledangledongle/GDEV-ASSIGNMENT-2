using System.Collections;
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemyList = new();
        foreach (GameObject enemyObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyList.Add(enemyObject.GetComponent<Enemy>());
        }

        setupBattle();
        UpdateHud();
        AttackAction.OnAttack += UpdateHud;
        DefendAction.OnDefend += UpdateHud;
    }

   
    private void setupBattle()
    {
        player.currentEnergy = player.maxEnergy;
        Debug.Log(player.currentEnergy);
        foreach (Enemy enemy in enemyList)
        {
            enemy.currentHP = enemy.maxHP;
        }
        //START FIRST TURN STUFF
    }

    private void UpdateHud()
    {
        hudHandler.SetHUD(player,enemyList);
    }



    
}
