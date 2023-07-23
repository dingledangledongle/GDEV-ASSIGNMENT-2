using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public float currentDef;
    public float defValue;
    public float dmg;
    public bool isShielded;

    private void Start()
    {
    }
    /*public void Attack(Player target)
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

    }*/

    private void Defend(Enemy target)
    {
        target.isShielded = true;
        target.currentDef += defValue;
    }



    #region Damage Calculation
    //DAMAGE CALCULATION PART
    public void TakeDamage(DamageType damage)
    {
        if (isShielded)
        {
            float outstandingDmg = CalculateShieldDamage(damage);
            if (outstandingDmg > 0)
            {
                ReduceHealth(outstandingDmg);
            }
        }
        else
        {
            CalculateHealthDamage(damage);
        }
    }

    private float CalculateShieldDamage(DamageType damage)
    {
        for (int i = 0; i < damage.NumberOfHits; i++)
        {
            float outstandingDmg = ReduceShield(damage.DamagePerHit);
            if (outstandingDmg > 0)
            {
                return outstandingDmg;
            }
        }
        return 0;
    }
    private float ReduceShield(float dmgTaken)
    {
        float outstandingDmg = Math.Max(dmgTaken - currentDef, 0);
        currentDef = Math.Max(currentDef - dmgTaken, 0);
        if (currentDef < 0)
        {
            isShielded = false;
        }
        return outstandingDmg;
    }

    private void CalculateHealthDamage(DamageType damage)
    {
        for (int i = 0; i < damage.NumberOfHits; i++)
        {
            ReduceHealth(damage.DamagePerHit);
        }
    }
    private void ReduceHealth(float dmgTaken)
    {
        currentHP = Math.Max(currentHP - dmgTaken, 0);
    }
    #endregion
}
