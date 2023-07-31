using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceHandler : MonoBehaviour
{
    //VARIABLES
    public GameObject dicePrefab;

    private int numOfDice;
    private List<Dice> diceList;
    //EVENTS
    public static event Func<int> OnDiceBoxSpawn;//Player.GetNumberOfDice()
    public static event Func<Dictionary<string,int>> OnAllDiceLanded; //MaterialAction.UpdateMaterialList()
    public static event Action OnMaterialListUpdated;
    private void Awake()
    {
        diceList = new();

        //EVENTS SUBSCRIBING
        StartPlayerState.OnDiceFinish += IsAllDiceStationary;
        StartPlayerState.OnMaterialListUpdate += UpdateMaterialList;
        MaterialAction.OnUpdateMaterialUI += DestroyThis;

        numOfDice = OnDiceBoxSpawn.Invoke();
        SpawnDice();
    }

    private void OnDestroy()
    {
        StartPlayerState.OnDiceFinish -= IsAllDiceStationary;
        StartPlayerState.OnMaterialListUpdate -= UpdateMaterialList;
        MaterialAction.OnUpdateMaterialUI -= DestroyThis;
    }

    private void SpawnDice()
    {
        
        Vector3 spawnPos = new(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z -20);
        for (int i = 0; i < numOfDice; i++)
        {
            GameObject dice = Instantiate(dicePrefab, this.transform.Find("Wall/SpawnPosition")) ;
            //dice.transform.parent = this.transform.Find("Wall/SpawnPosition");
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
        Dictionary<string, int> matList = OnAllDiceLanded.Invoke();
        foreach (Dice dice in diceList)
        {
            matList[dice.Material] += 1;
        }
        OnMaterialListUpdated.Invoke();
    }


    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
