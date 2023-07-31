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

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //ENCOUNTER EVENTS
        EncounterManager.OnBattleStart += SetupBattle;

        AttackAction.OnAttackSuccess += UpdateHud;
        DefendAction.OnUpdateHud += UpdateHud;
        MaterialAction.OnAfterEnhance += UpdateHud;
        //START BATTLE EVENTS
        StartBattleState.OnBattleStart += SetupBattle;
        //START PLAYER EVENTS
        StartPlayerState.OnRollDice += RollDice;
        StartPlayerState.OnDisplayReady += UpdateHud;
        //START ENEMY EVENTS
        StartEnemyState.OnEnemyStart += OnEnemyStart;
        StartEnemyState.OnEnemyFinishAction += IsEnemyDone;
        StartEnemyState.OnEnemyAction += UpdateHud;

        //PLAYER EVENTS
        Player.OnPlayerDamageTaken += UpdateHud;

        //ENEMY EVENTS
        Enemy.OnEnemyDeath += CheckAllEnemyDeath;
        Enemy.OnActionFinished += UpdateHud;
    }

    private void OnDestroy()
    {
        EncounterManager.OnBattleStart -= SetupBattle;
        AttackAction.OnAttackSuccess -= UpdateHud;
        DefendAction.OnDefend -= UpdateHud;
        MaterialAction.OnAfterEnhance -= UpdateHud;

        //START PLAYER EVENTS
        StartPlayerState.OnRollDice -= RollDice;
        StartPlayerState.OnDisplayReady -= UpdateHud;
        //START ENEMY EVENTS
        StartEnemyState.OnEnemyStart -= OnEnemyStart;
        StartEnemyState.OnEnemyFinishAction -= IsEnemyDone;
        StartEnemyState.OnEnemyAction -= UpdateHud;

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
