using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy :MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public float def;
    public float dmg;
    public bool isShielded;
 

    public void Attack(Player target)
    {
        float hpDmg = this.dmg;
        if (target.isShielded)
        {
            hpDmg = this.dmg - target.currentDef;
            target.currentDef = Mathf.Max(target.currentDef - this.dmg, 0);
            //change to observer?
            if (target.currentDef == 0)
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
