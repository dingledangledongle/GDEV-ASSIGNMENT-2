using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public float maxHP = 100;
    public float currentHP;
    public float currentDef = 0;
    public float dmg = 5;
    public float defValue = 5;
    public bool isShielded = false;
    public int currentEnergy;
    public int maxEnergy = 3;
    
    private void Start()
    {
        currentHP = this.maxHP;
        AttackAction.OnTargetGet += Attack;
        AttackAction.OnAfterAttack += ReduceCurrentEnergy;
        DefendAction.OnDefend += Defend;


    }
    public void Attack(Enemy target)
    {
        float hpDmg = this.dmg;
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
            target.currentHP = Math.Max(target.currentHP - hpDmg, 0);
        }

    }

    public void Defend() 
    {
        this.isShielded = true;
        this.currentDef += defValue;
    }

    public void ReduceCurrentEnergy(int energyCost)
    {
        if(currentEnergy!= 0)
        {
            Debug.Log("MINUS" + currentEnergy);
            currentEnergy -= energyCost;
        }
    }

}
