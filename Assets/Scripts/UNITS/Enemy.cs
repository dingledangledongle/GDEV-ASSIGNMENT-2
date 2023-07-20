using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    string[] moveSet = { "attack", "defend" };


    public string GenerateIntent()
    {
        int intNum = Random.Range(0, 2);
        string intent = moveSet[intNum];
        return intent;
    }

    public void Attack(Unit self, Unit target)
    {
        target.currentHP -= self.dmg;
    }


}
