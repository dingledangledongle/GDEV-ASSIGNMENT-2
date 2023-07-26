using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class Player : MonoBehaviour
{
    #region VARIABLES
    // MAP VARIABLES
    private int currentNode;

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
    private Animator animator;
    private Dictionary<string, int> materialList;
    public GameObject FloatText;
    public GameObject SlashEffect;

    #endregion

    public static event Action OnPlayerDamageTaken;
    private void Start()
    {
        currentHP = this.maxHP;
        currentDefValue = baseDefValue;
        animator = this.gameObject.GetComponent<Animator>();
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
        AttackAction.OnAttackSuccess += PlayAttackAnim;
        DefendAction.OnDefend += Defend;
        DefendAction.OnDefend += PlayDefendAnim;
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

    private void PlayAttackAnim()
    {
        animator.Play("HeroKnight_Attack1");
    }
    private void PlayDefendAnim()
    {
        animator.Play("HeroKnight_IdleBlock");
    }

    private bool CheckPlayerDeath()
    {
        if (currentHP <= 0)
        {
            animator.Play("HeroKnight_Death");
            return true;
        }
        return false;
    }
    private void ShowFloatingText(string text)
    {
        Vector3 spawnPos = new(transform.position.x, transform.position.y + 10);
        GameObject floatText = Instantiate(SlashEffect, spawnPos, Quaternion.identity);
        floatText.GetComponent<TMP_Text>().text = text;
    }

    private void ShowSlashEffect()
    {
        Vector3 spawnPos = new(transform.position.x, transform.position.y);
        Instantiate(FloatText, spawnPos, Quaternion.identity, transform.Find("Canvas"));
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
            animator.Play("HeroKnight_Block");
        }
        else
        {
            StartCoroutine(CalculateHealthDamage(damage));
            animator.Play("HeroKnight_Hurt");

        }
        CheckPlayerDeath();
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

    private IEnumerator CalculateHealthDamage(DamageType damage)
    {
        for (int i = 0; i < damage.NumberOfHits; i++)
        {
            ReduceHealth(damage.DamagePerHit);
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void ReduceHealth(float dmgTaken)
    {
        currentHP = Math.Max(currentHP - dmgTaken, 0);
        ShowFloatingText(dmgTaken.ToString());
        ShowSlashEffect();
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

    private int GetCurrentNode()
    {
        return currentNode;
    }

    #region GETTER / SETTER\
    public int CurrentNode
    {
        get
        {
            return currentNode;
        }
        set {
            currentNode = value;
        }
    }
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
