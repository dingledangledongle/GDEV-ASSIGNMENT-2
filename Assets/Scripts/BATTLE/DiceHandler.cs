using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DiceHandler : MonoBehaviour
{
    //VARIABLES
    public GameObject dicePrefab;

    private int numOfDice;
    private List<Dice> diceList = new();
    //EVENTS
    public static event Func<int> OnDiceBoxSpawn;//Player.GetNumberOfDice()
    public static event Func<Dictionary<string,int>> OnAllDiceLanded; //MaterialAction.UpdateMaterialList()
    public static event Action OnMaterialListUpdated;
    private void Start()
    {
        StartPlayerState.OnDiceFinish += IsAllDiceStationary;
        StartPlayerState.OnMaterialListUpdate += UpdateMaterialList;
        numOfDice = OnDiceBoxSpawn.Invoke();
        SpawnDice();
    }

    private void SpawnDice()
    {
        Vector3 spawnPos = new(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 100);
        for (int i = 0; i < numOfDice; i++)
        {
            GameObject dice = Instantiate(dicePrefab,spawnPos,Quaternion.identity) ;
            diceList.Add(dice.GetComponent<Dice>());
        }
    }

    private bool IsAllDiceStationary()
    {
        Debug.Log("checking dice stationary");
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
        Debug.Log("DICEHANDLER updateLIST");

        Dictionary<string, int> matList = OnAllDiceLanded.Invoke();
        foreach (Dice dice in diceList)
        {
            matList[dice.Material] += 1;
        }
        OnMaterialListUpdated.Invoke();
    }


}
