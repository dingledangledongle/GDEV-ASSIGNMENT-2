using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceHandler : MonoBehaviour
{
    //VARIABLES
    public GameObject dicePrefab;

    private int numOfDice;
    private List<Dice> diceList;
    private EventManager eventManager = EventManager.Instance;
    //EVENTS
    private void Awake()
    {
        diceList = new();

        //EVENTS SUBSCRIBING
        eventManager.AddListener<bool>(Event.PLAYER_ROLLDICE,IsAllDiceStationary);
        eventManager.AddListener(Event.PLAYER_DICE_FINISHED, UpdateMaterialList);
        eventManager.AddListener(Event.PLAYER_MATERIALUPDATED, DestroyThis);        

        numOfDice = eventManager.TriggerEvent<int>(Event.PLAYER_DICE);
        SpawnDice();
    }

    private void OnDestroy()
    {
        eventManager.RemoveListener<bool>(Event.PLAYER_ROLLDICE, IsAllDiceStationary);
        eventManager.RemoveListener(Event.PLAYER_DICE_FINISHED, UpdateMaterialList);
        eventManager.RemoveListener(Event.PLAYER_MATERIALUPDATED, DestroyThis);
    }

    private void SpawnDice()
    {
        
        Vector3 spawnPos = new(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z -20);
        for (int i = 0; i < numOfDice; i++)
        {
            GameObject dice = Instantiate(dicePrefab, this.transform.Find("Wall/SpawnPosition")) ;
            diceList.Add(dice.GetComponent<Dice>());
        }
    }

    private bool IsAllDiceStationary()
    {
        int confirmed = 0;
        foreach (Dice dice in diceList)
        {
            if (dice.HasLanded)
            {
                confirmed += 1;
            }
        }

        if(confirmed == numOfDice)
        {
            return true;
        }
        return false;
    }

    private void UpdateMaterialList()
    {
        Dictionary<string, int> matList = eventManager.TriggerEvent<Dictionary<string,int>>(Event.PLAYER_DICE);
        foreach (Dice dice in diceList)
        {
            matList[dice.Material] += 1;
        }
        eventManager.TriggerEvent(Event.PLAYER_DICE);
    }


    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
