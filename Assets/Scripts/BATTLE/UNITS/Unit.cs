using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //no longer used

    /* the plan was to make the player and enemy inherit this class's properties and methods
     * as both of them has the same basic few methods that they share
     * 
    */

    public int maxHP = 10;
    public int currentHP;
    public int def = 0;
    public int dmg = 2;
    public bool isShielded = false;

    public void Attack(Unit target)
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

