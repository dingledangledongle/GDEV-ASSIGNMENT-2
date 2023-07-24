using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    private float currentHP;
    private float currentDef;
    private bool isShielded = false;
    private EnemyMoves moveSet;
    private Move currentMove;

    public static event Action OnGetIntent;
    public static event Action<DamageType> OnEnemyAttack;
    private void Start()
    {
        currentHP = maxHP;
        moveSet = this.gameObject.GetComponent<EnemyMoves>();
        StartPlayerState.OnPlayerStart += GetIntent;
    }

    private void GetIntent()
    {
        currentMove = moveSet.GetMove();
        Debug.Log(currentMove + this.transform.name);
        OnGetIntent?.Invoke();
    }
    public void PerformAction()
    {
        if (currentMove.MoveType.Equals(Move.Type.ATTACK))
        {
            OnEnemyAttack?.Invoke(currentMove.Damage);
        }
    }

    private void Defend(float def) {
        currentDef += def;
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

    public Move CurrentMove
    {
        get
        {
            return currentMove;
        }
    }
    #endregion
}
