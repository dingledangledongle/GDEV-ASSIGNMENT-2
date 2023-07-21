using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public int def = 0;
    public int dmg = 5;
    public bool isShielded = false;
    public int currentEnergy;
    public int maxEnergy = 3;

    public void Attack(Enemy target)
    {
        int hpDmg = this.dmg;
        if (target.isShielded)
        {
            hpDmg = this.dmg - target.def;
            target.def = Math.Max(target.def - this.dmg, 0);

            //change to observer?
            if (target.def == 0)
            {
                target.isShielded = false;
            }

            target.currentHP = Math.Max(target.currentHP - hpDmg, 0);
        }
        else
        {
            target.currentHP -= hpDmg;
        }

    }

    public void Defend(Unit target)
    {
        target.isShielded = true;
        target.def += 3;
    }

}
