using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class BattleManager : MonoBehaviour
{
    private HUDHandler hudHandler;
    private Player player;
    private List<Enemy> enemyList;
    private bool enemyDone = false;

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
        StartEnemyState.OnEnemyFinishAction += IsEnemyDone;
        StartEnemyState.OnEnemyStart += OnEnemyStart;
        StartEnemyState.OnEnemyAction += UpdateHud;

        Player.OnPlayerDamageTaken += UpdateHud;
        setupBattle();
    }

    private void OnEnemyStart()
    {
        enemyDone = false;
        StartCoroutine(PerformEnemyActions());
    }
    private bool IsEnemyDone()
    {
        Debug.Log(enemyDone);
        return enemyDone;
    }
    private IEnumerator PerformEnemyActions()
    {
        foreach (Enemy enemy in enemyList)
        {
            enemy.PerformAction();
            yield return new WaitForSeconds(1);
        }
        enemyDone = true;
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
