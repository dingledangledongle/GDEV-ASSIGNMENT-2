using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy :MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public int def;
    public int dmg;
    public bool isShielded;
 

    public void Attack(Player target)
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

    public void Defend(Enemy target)
    {
        target.isShielded = true;
        target.def += 3;
    }

}
