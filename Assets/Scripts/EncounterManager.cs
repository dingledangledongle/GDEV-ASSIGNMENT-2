using System;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    GameObject[] battleObject;
    GameObject[] restObject;
    GameObject[] eventObject;

    public static event Action OnBattleStart; //BattleManager.SetupBattle()
    public static event Action OnBattleState; //BattleStateManager.StartBattle()
    public static event Action OnMapToggle;
    private void Start()
    {
        battleObject = GameObject.FindGameObjectsWithTag("Battle");
        SetInactive(battleObject);

        restObject = GameObject.FindGameObjectsWithTag("Rest");


        eventObject = GameObject.FindGameObjectsWithTag("Events");

        //EVENTS
        EventManager.Instance.AddListener<Node>(Event.MAP_NODE_CLICKED,StartEncounter);
        EndBattleState.OnBattleEnd += EndBattle;
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<Node>(Event.MAP_NODE_CLICKED, StartEncounter);
        EndBattleState.OnBattleEnd += EndBattle;
    }
    private void StartEncounter(Node node)
    {
        Debug.Log("start encounter");
        switch (node.EncounterType)
        {
            case Node.Encounter.ENEMY:
                StartBattle(node);
                OnBattleStart?.Invoke();
                break;
            case Node.Encounter.ELITE:
                StartBattle(node);
                OnBattleStart?.Invoke();
                break;
            case Node.Encounter.EVENT:
                break;
            case Node.Encounter.REST:
                break;
            case Node.Encounter.CHEST:
                break;
            case Node.Encounter.BOSS:
                StartBattle(node);
                OnBattleStart?.Invoke();
                break;
        }

        //DISABLE MAP CLICKY
        //CLOSE MAP
    }
    
    private void SetInactive(GameObject[] list)
    {
        foreach (GameObject item in list)
        {
            item.SetActive(false);
        }
    }

    private void StartRest()
    {
        Debug.Log("start rest");

        foreach (GameObject item in restObject)
        {
            item.SetActive(true);
        }
    }

    #region BATTLE
    private void StartBattle(Node node)
    {
        Debug.Log("start battle");
        //SET ALL BATTLE-RELATED OBJECTS TO ACTIVE
        foreach (GameObject item in battleObject)
        {
            item.SetActive(true);
        }

        //spawn enemy in the container
        GameObject enemyContainer = GameObject.Find("EnemySpot");
        Debug.Log(enemyContainer.name);
        foreach (GameObject enemy in node.EnemyList)
        {
            Instantiate(enemy, enemyContainer.transform);
        }
        OnBattleState?.Invoke();

        //close the map
        OnMapToggle?.Invoke();
    }

    private void EndBattle()
    {
        //remove enemies
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyList)
        {
            Destroy(enemy);
        }
        //hide battle ui
        foreach (GameObject item in battleObject)
        {
            item.SetActive(false);
        }

        //open map
        OnMapToggle?.Invoke();
    }
    #endregion
}
