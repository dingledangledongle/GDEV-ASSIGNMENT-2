using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region VARIABLES
    //HEALTH + SHIELD VARAIBLES
    private float maxHP = 100f;
    private float currentHP;
    private float currentDef = 0f;
    private float baseDefValue = 5f;
    private float currentDefValue;
    private bool isShielded = false;
     
    //DAMAGE VARIABLE
    private float baseDmg = 5f;
    private int baseNumberOfHits = 1;

    //ENERGY VARIABLE
    private int currentEnergy;
    private int maxEnergy = 3;

    private DamageType damage;
    private Dictionary<string, int> materialList;
    #endregion

    public static event Action OnPlayerDamageTaken;
    private void Start()
    {
        currentHP = this.maxHP;
        currentDefValue = baseDefValue;

        materialList = new();
        damage = new(baseDmg,baseNumberOfHits);

        #region Event Subscribing
        // TURN EVENTS
        StartPlayerState.OnPlayerStart += TurnStart;
        EndPlayerState.OnPlayerEnd += TurnEnd;

        // ATTACK / DEF ACTION
        AttackAction.OnTargetGet += GetDamage;
        AttackAction.OnAfterAttack += ReduceCurrentEnergy;
        AttackAction.OnAttackSuccess += ResetValues;
        DefendAction.OnDefend += Defend;
        DefendAction.OnAfterDef += ReduceCurrentEnergy;

        // MATERIAL ACTION
        MaterialAction.OnAttackEnhance += ModifyDamage;
        MaterialAction.OnDefEnhance += ModifyDefense;
        MaterialAction.OnSuccessEnhance += ReduceCurrentEnergy;

        //ENEMY EVENTS
        Enemy.OnEnemyAttack += TakeDamage;
        #endregion
    }
  
    private void Defend()
    {
        this.isShielded = true;
        this.currentDef += currentDefValue;
        currentDefValue = baseDefValue;
    }
    private void ReduceCurrentEnergy(int energyCost)
    {
        if (currentEnergy != 0)
        {
            currentEnergy -= energyCost;
        }
    }


    #region Damage Calculation
    private void TakeDamage(DamageType damage)
    {
        if (isShielded)
        {
            float outstandingDmg = CalculateShieldDamage(damage);
            if(outstandingDmg > 0)
            {
                ReduceHealth(outstandingDmg);
            }
        }
        else
        {
            CalculateHealthDamage(damage);
        }
        OnPlayerDamageTaken?.Invoke();
    }

    private float CalculateShieldDamage(DamageType damage)
    {
        for (int i = 0; i < damage.NumberOfHits; i++)
        {
            float outstandingDmg = ReduceShield(damage.DamagePerHit);
            if(outstandingDmg > 0)
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
   
    #region Damage/Defense Modifying
    private void ModifyDamage(float modifier,int numOfHits)
    {
        damage.NumberOfHits = numOfHits;
        damage.DamagePerHit += modifier;
    }

    private void ModifyDefense(float modifier)
    {
        currentDefValue += modifier;
    }
    #endregion

    #region Turn Actions
    private void TurnStart()
    {
        currentEnergy = maxEnergy;
        if (isShielded)
        {
            currentDef = 0;
            isShielded = false;
        }

        //PLAY OUT TURN START EFFECTS
    }

    private void TurnEnd()
    {
        //END TURN EVENTS
        //E.G. POISON 

        Debug.Log("player turn end");
    }

    private void ResetValues()
    {
        damage.DamagePerHit = baseDmg;
        damage.NumberOfHits = baseNumberOfHits;
    }

    #endregion

    #region GETTER / SETTER
    public float CurrentHP
    {
        get 
        {
            return currentHP;
        }
    }
    public float MaxHP
    {
        get
        {
            return maxHP;
        }
    }

    public float CurrentDef
    {
        get
        {
            return currentDef;
        }
    }
    public bool IsShielded
    {
        get
        {
            return isShielded;
        }
    }

    public int CurrentEnergy
    {
        get
        {
            return currentEnergy;
        }
    }

    public int MaxEnergy
    {
        get
        {
            return maxEnergy;
        }
    }

    public float CurrentDmg
    {
        get
        {
            return damage.DamagePerHit;
        }
    }
    public int NumberOfHits
    {
        get
        {
            return damage.NumberOfHits;
        }
    }

    public float CurrentDefValue
    {
        get
        {
            return currentDefValue;
        }
    }

    public DamageType GetDamage()
    {
        return damage;
    }
    #endregion
}
