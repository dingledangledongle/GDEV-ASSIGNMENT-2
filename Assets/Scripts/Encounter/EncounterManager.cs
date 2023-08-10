using System;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    private GameObject[] battleObjects;
    private GameObject[] restObjects;
    private GameObject[] eventObjects;
    private Node.Encounter currentEncounter;

    private EventManager eventManager = EventManager.Instance; 

    private void Start()
    {
        //setting references to the gameobjects and setting them to inactive
        battleObjects = GameObject.FindGameObjectsWithTag("Battle");
        SetInactive(battleObjects);

        restObjects = GameObject.FindGameObjectsWithTag("Rest");
        SetInactive(restObjects);

        eventObjects = GameObject.FindGameObjectsWithTag("Events");
        SetInactive(eventObjects);

        //EVENTS
        eventManager.AddListener<Node>(Event.MAP_NODE_CLICKED,StartEncounter);
        eventManager.AddListener(Event.REST_FINISHED, EndRest);
        eventManager.AddListener(Event.BATTLE_END, EndBattle);
        eventManager.AddListener<Node.Encounter>(Event.ENEMY_DEATH,GetCurrentEncounter);
    }

    private void OnDestroy()
    {
        eventManager.RemoveListener<Node>(Event.MAP_NODE_CLICKED, StartEncounter);
        eventManager.RemoveListener(Event.REST_FINISHED, EndRest);
        eventManager.RemoveListener(Event.BATTLE_END, EndBattle);
        eventManager.RemoveListener<Node.Encounter>(Event.ENEMY_DEATH, GetCurrentEncounter);

    }
    private void StartEncounter(Node node)
    {
        Debug.Log("start encounter");
        currentEncounter = node.EncounterType;

        //determining the node's encounter and start the appropriate encounters
        switch (node.EncounterType)
        {
            case Node.Encounter.ENEMY:
                StartBattle(node);
                eventManager.TriggerEvent(Event.BATTLE_START);
                break;

            case Node.Encounter.ELITE:
                StartBattle(node);
                eventManager.TriggerEvent(Event.BATTLE_START);
                break;

            case Node.Encounter.EVENT:
                StartRandomEvent(node);
                break;

            case Node.Encounter.REST:
                StartRest(node);
                break;

            case Node.Encounter.BOSS:
                StartBattle(node);
                eventManager.TriggerEvent(Event.BATTLE_START);
                break;
        }

        //DISABLE MAP CLICKY
    }
    
    private void SetInactive(GameObject[] list)
    {
        foreach (GameObject item in list) // iterates through the list and setting the objects to inactive
        {
            item.SetActive(false);
        }
    }

    #region Random Event
    private void StartRandomEvent(Node node)
    {
        foreach (GameObject item in eventObjects) // set all the event gameobject to active
        {
            item.SetActive(true);
        }

        //trigger the initialize method in RandomEventHandler class
        eventManager.TriggerEvent<Node>(Event.RAND_EVENT_INITIALIZE, node);

    }
    #endregion

    #region Rest
    private void StartRest(Node node)
    {
        Debug.Log("start rest");
        //enabling all rest ui
        foreach (GameObject item in restObjects)
        {
            Debug.Log(item.name);
            item.SetActive(true);
        }

        //initialize rest event
        eventManager.TriggerEvent<Node>(Event.REST_INITIALIZE,node);

        //trigger method that disables node in the same depth and closes the map
        eventManager.TriggerEvent(Event.MAP_NODE_CLICKED);
    }

    private void EndRest()
    {
        //DISABLING ALL REST UI
        SetInactive(restObjects);

        //open map
        eventManager.TriggerEvent(Event.MAP_NODE_CLICKED);
    }
    #endregion

    #region BATTLE
    private void StartBattle(Node node)
    {
        Debug.Log("start battle");
        //SET ALL BATTLE-RELATED OBJECTS TO ACTIVE
        foreach (GameObject item in battleObjects)
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

        //close the map
        eventManager.TriggerEvent(Event.MAP_NODE_CLICKED);
    }

    private void EndBattle()
    {
        //remove enemies
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        
        //removing all enemy object
        foreach (GameObject enemy in enemyList)
        {
            Destroy(enemy);
        }

        //hide battle ui
        foreach (GameObject item in battleObjects)
        {
            item.SetActive(false);
        }

        //open map
        eventManager.TriggerEvent(Event.MAP_NODE_CLICKED);
    }
    #endregion


    private Node.Encounter GetCurrentEncounter()
    {
        return currentEncounter;
    }
}
