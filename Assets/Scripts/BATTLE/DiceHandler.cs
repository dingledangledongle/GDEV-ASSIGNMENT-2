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

    private void Awake()
    {
        diceList = new();

        // subscribing events
        eventManager.AddListener<bool>(Event.PLAYER_ROLLDICE,IsAllDiceStationary);
        eventManager.AddListener(Event.PLAYER_DICE_FINISHED, UpdateMaterialList);
        eventManager.AddListener(Event.PLAYER_MATERIALUPDATED, DestroyThis);        

        //getting the number of dice
        numOfDice = eventManager.TriggerEvent<int>(Event.PLAYER_DICE);

        //spawns dice the moment the prefab is spawned
        SpawnDice();
    }

    private void OnDestroy()
    {
        // unsubscribing events
        eventManager.RemoveListener<bool>(Event.PLAYER_ROLLDICE, IsAllDiceStationary);
        eventManager.RemoveListener(Event.PLAYER_DICE_FINISHED, UpdateMaterialList);
        eventManager.RemoveListener(Event.PLAYER_MATERIALUPDATED, DestroyThis);
    }

    private void SpawnDice()
    {
        //spawns multiple dice at the spawn point that was placed in the prefab
        for (int i = 0; i < numOfDice; i++)
        {
            GameObject dice = Instantiate(dicePrefab, this.transform.Find("Wall/SpawnPosition")) ;
            diceList.Add(dice.GetComponent<Dice>());
        }
    }

    private bool IsAllDiceStationary()
    {
        int confirmed = 0; // counter for the amount of dice that are stationary

        //check through each dice if they have landed and adds 1 to the counter if they are
        foreach (Dice dice in diceList)
        {
            if (dice.HasLanded)
            {
                confirmed += 1;
            }
        }

        // return true once all the dice are stationary
        if(confirmed == numOfDice)
        {
            return true;
        }
        return false;
    }

    private void UpdateMaterialList()
    {
        //get the materials the player currently has
        Dictionary<string, int> matList = eventManager.TriggerEvent<Dictionary<string,int>>(Event.PLAYER_DICE);

        //checks through the list of dice and adds 1 to the corresponding material the dice landed on
        foreach (Dice dice in diceList)
        {
            matList[dice.Material] += 1;
        }

        //trigger the methods responsible for ending the dice roll
        eventManager.TriggerEvent(Event.PLAYER_DICE);
    }


    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
