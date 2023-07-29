using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    GameObject[] battleObject;

    private void Start()
    {
        battleObject = GameObject.FindGameObjectsWithTag("Battle");
        SetInactive(battleObject);
        NodeObject.OnClick += StartEncounter;
    }
    private void StartEncounter(Node node)
    {
        Debug.Log("start encounter");
        switch (node.EncounterType)
        {
            case Node.Encounter.ENEMY:
                StartBattle(node);
                break;
            case Node.Encounter.ELITE:
                break;
            case Node.Encounter.EVENT:
                break;
            case Node.Encounter.REST:
                break;
            case Node.Encounter.CHEST:
                break;
        }

        //DISABLE MAP CLICKY
        //CLOSE MAP
    }
    private void StartBattle(Node node)
    {
        //list of enemies
        Debug.Log("start battle");
        //SET ALL BATTLE-RELATED OBJECTS TO ACTIVE
        foreach (GameObject item in battleObject)
        {
            item.SetActive(true);
        }

        GameObject enemyContainer = GameObject.Find("EnemySpot");
        Debug.Log(enemyContainer.name);
        foreach (GameObject enemy in node.EnemyList)
        {
            Instantiate(enemy, enemyContainer.transform);
        }

    }

    private void SetInactive(GameObject[] list)
    {
        foreach (GameObject item in list)
        {
            item.SetActive(false);
        }
    }

}
