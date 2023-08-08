using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    public GameObject diceBoxPrefab;
    private HUDHandler hudHandler;
    private Player player;
    private List<Enemy> enemyList;
    private bool enemyDone = false;
    private EventManager eventManager = EventManager.Instance;

    private void Awake()
    {
        hudHandler = new();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //ENCOUNTER EVENTS
        eventManager.AddListener(Event.BATTLE_START, SetupBattle);
        eventManager.AddListener(Event.UPDATE_HUD, UpdateHud);

        // player events
        eventManager.AddListener(Event.PLAYER_ATTACK_FINISHED, UpdateHud);
        eventManager.AddListener(Event.PLAYER_DEFEND_FINISHED, UpdateHud);
        eventManager.AddListener(Event.PLAYER_ENHANCE_SUCCESS, UpdateHud);
        eventManager.AddListener(Event.PLAYER_ROLLDICE, RollDice);
        eventManager.AddListener(Event.PLAYER_TAKEDAMAGE, UpdateHud);


        // enemy events
        eventManager.AddListener(Event.ENEMY_TURN, OnEnemyStart);
        eventManager.AddListener(Event.ENEMY_TURN, UpdateHud);
        eventManager.AddListener<bool>(Event.ENEMY_TURN, IsEnemyDone);
        eventManager.AddListener<bool>(Event.ENEMY_DEATH, CheckAllEnemyDeath);
    }

    private void OnDestroy()
    {
        //ENCOUNTER EVENTS
        eventManager.RemoveListener(Event.BATTLE_START, SetupBattle);
        eventManager.RemoveListener(Event.UPDATE_HUD, UpdateHud);

        //PLAYER EVENTS
        eventManager.RemoveListener(Event.PLAYER_ATTACK_FINISHED, UpdateHud);
        eventManager.RemoveListener(Event.PLAYER_DEFEND_FINISHED, UpdateHud);
        eventManager.RemoveListener(Event.PLAYER_ENHANCE_SUCCESS, UpdateHud);
        eventManager.RemoveListener(Event.PLAYER_ROLLDICE, RollDice);
        eventManager.RemoveListener(Event.PLAYER_TAKEDAMAGE, UpdateHud);

        //ENEMY EVENTS
        eventManager.RemoveListener(Event.ENEMY_TURN, OnEnemyStart);
        eventManager.RemoveListener(Event.ENEMY_TURN, UpdateHud);
        eventManager.RemoveListener<bool>(Event.ENEMY_TURN, IsEnemyDone);
        eventManager.RemoveListener<bool>(Event.ENEMY_DEATH, CheckAllEnemyDeath);


    }

    private void RollDice()
    {
        Debug.Log("rolling dice");
        Vector3 spawnPos = new(transform.position.x, transform.position.y +18, transform.position.z - 100);
        GameObject diceBox = Instantiate(diceBoxPrefab, GameObject.Find("Canvas").transform,false);
    }

    #region ENEMY FUNCTIONS
    private void OnEnemyStart()
    {
        enemyDone = false;
        foreach(Enemy enemy in enemyList)
        {
            enemy.TurnStart();
        }
        StartCoroutine(PerformEnemyActions());
    }

    private bool IsEnemyDone()
    {
        return enemyDone;
    }

    private IEnumerator PerformEnemyActions()
    {
        foreach (Enemy enemy in enemyList)
        {
            if (!enemy.IsDead) {
                enemy.PerformAction();
                yield return new WaitForSeconds(1);
            }
        }
        enemyDone = true;
    }

    private bool CheckAllEnemyDeath()
    {
        foreach (Enemy enemy in enemyList)
        {
            if (!enemy.IsDead)
            {
                return false;
            }
        }
        return true;
    }
    #endregion
    private void SetupBattle()
    {
        enemyList = new();
        foreach (GameObject enemyObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyList.Add(enemyObject.GetComponent<Enemy>());
        }
    }

    private void UpdateHud()
    {
        hudHandler.SetHUD(player, enemyList);
    }
}
