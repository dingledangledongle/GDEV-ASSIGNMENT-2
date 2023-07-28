using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class BattleManager : MonoBehaviour
{
    public GameObject diceBoxPrefab;
    private HUDHandler hudHandler;
    private Player player;
    private List<Enemy> enemyList;
    private bool enemyDone = false;

    private void Awake()
    {
        hudHandler = new();
        enemyList = new();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        foreach (GameObject enemyObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyList.Add(enemyObject.GetComponent<Enemy>());
        }

        AttackAction.OnAttackSuccess += UpdateHud;
        DefendAction.OnUpdateHud += UpdateHud;
        MaterialAction.OnAfterEnhance += UpdateHud;

        //START PLAYER EVENTS
        StartPlayerState.OnPlayerStart += RollDice;
        StartPlayerState.OnDisplayReady += UpdateHud;
        //START ENEMY EVENTS
        StartEnemyState.OnEnemyStart += OnEnemyStart;
        StartEnemyState.OnEnemyFinishAction += IsEnemyDone;
        StartEnemyState.OnEnemyAction += UpdateHud;

        //PLAYER EVENTS
        Player.OnPlayerDamageTaken += UpdateHud;
    }

    private void OnDestroy()
    {
        AttackAction.OnAttackSuccess -= UpdateHud;
        DefendAction.OnDefend -= UpdateHud;
        MaterialAction.OnAfterEnhance -= UpdateHud;

        //START PLAYER EVENTS
        StartPlayerState.OnPlayerStart -= RollDice;
        StartPlayerState.OnDisplayReady -= UpdateHud;
        //START ENEMY EVENTS
        StartEnemyState.OnEnemyStart -= OnEnemyStart;
        StartEnemyState.OnEnemyFinishAction -= IsEnemyDone;
        StartEnemyState.OnEnemyAction -= UpdateHud;

        //PLAYER EVENTS
        Player.OnPlayerDamageTaken -= UpdateHud;
    }
    private void Start()
    {
        setupBattle();

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
    #endregion
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
