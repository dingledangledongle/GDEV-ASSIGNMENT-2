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

        eventManager.AddListener(Event.PLAYER_ATTACK_FINISHED, UpdateHud);
        eventManager.AddListener(Event.PLAYER_DEFEND_FINISHED, UpdateHud);
        eventManager.AddListener(Event.PLAYER_ENHANCE_SUCCESS, UpdateHud);
        eventManager.AddListener(Event.UPDATE_HUD,UpdateHud);

        //START BATTLE EVENTS
        StartBattleState.OnBattleStart += SetupBattle;

        //START PLAYER EVENTS
        eventManager.AddListener(Event.PLAYER_ROLLDICE, RollDice);

        //START ENEMY EVENTS
        eventManager.AddListener(Event.ENEMY_TURN, OnEnemyStart);
        eventManager.AddListener(Event.ENEMY_TURN, UpdateHud);
        eventManager.AddListener<bool>(Event.ENEMY_TURN, IsEnemyDone);

        //PLAYER EVENTS
        Player.OnPlayerDamageTaken += UpdateHud;

        //ENEMY EVENTS
        Enemy.OnEnemyDeath += CheckAllEnemyDeath;
        Enemy.OnActionFinished += UpdateHud;
    }

    private void OnDestroy()
    {
        EncounterManager.OnBattleStart -= SetupBattle;

        //START ENEMY EVENTS

        //PLAYER EVENTS
        Player.OnPlayerDamageTaken -= UpdateHud;

        //ENEMY EVENTS
        Enemy.OnEnemyDeath -= CheckAllEnemyDeath;
        Enemy.OnActionFinished -= UpdateHud;
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
