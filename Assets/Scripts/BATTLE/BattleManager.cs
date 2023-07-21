using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum State
{
    STATE_START,
    STATE_PLAYERSTART,
    STATE_PLAYERTURN,
    STATE_PLAYEREND,
    STATE_ENEMYSTART,
    STATE_ENEMYTURN,
    STATE_ENEMYEND,
    STATE_END     
}
public delegate void PlayerTurnStartEvent();
public delegate void PlayerTurnEndEvent();
public delegate void EnemyTurnStartEvent();
public delegate void EnemyTurnEndEvent();
public class BattleManager : MonoBehaviour
{
    private HUDHandler hudHandler;
    private Player player;
    private List<Enemy> enemyList;
    private State currentState = State.STATE_START;

    public event PlayerTurnStartEvent OnPlayerTurnStart;
    public event PlayerTurnEndEvent OnPlayerTurnEnd;
    public event EnemyTurnStartEvent OnEnemyTurnStart;
    public event EnemyTurnEndEvent OnEnemyTurnEnd;

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
    }

    private void StartPlayer()
    {
        currentState = State.STATE_PLAYERTURN;
        OnPlayerTurnStart?.Invoke();
    }
    private void EndPlayer()
    {
        currentState = State.STATE_PLAYEREND;
        OnPlayerTurnEnd?.Invoke();
    }
    private void StartEnemy()
    {
        currentState = State.STATE_ENEMYSTART;
        OnEnemyTurnStart?.Invoke();
    }
    private void EndEnemy()
    {
        currentState = State.STATE_ENEMYEND;
        OnEnemyTurnEnd?.Invoke();
    }
    private void setupBattle()
    {
        player.currentEnergy = player.maxEnergy;
        player.currentHP = player.maxHP;
        foreach (Enemy enemy in enemyList)
        {
            enemy.currentHP = enemy.maxHP;
        }
        //START FIRST TURN STUFF
        StartPlayer();
    }

    private void UpdateHud()
    {
        //hudHandler.setHUD(player,enemy);
        Debug.Log(player.transform.Find("HealthBar/HealthNum").name);
        
    }

    public void EndTurn()
    {
        if(currentState == State.STATE_PLAYERTURN)
        {
            EndPlayer();
            StartEnemy();
        }
    }

    
}
