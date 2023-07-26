using System;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public GameObject FloatText;
    public float maxHP;
    private float currentHP;
    private float currentDef;
    private bool isShielded = false;
    private EnemyMoves moveSet;
    private Move currentMove;
    
    public static event Action<DamageType> OnEnemyAttack;

    private void Awake()
    {
        currentHP = maxHP;
        moveSet = this.gameObject.GetComponent<EnemyMoves>();

        //EVENTS
        StartPlayerState.OnPlayerStart += GetIntent;
        StartEnemyState.OnEnterEnemyStart += TurnStart;
        Debug.Log(this.gameObject.name + " INITIALIZED");
    }

    private void OnDestroy()
    {
        StartPlayerState.OnPlayerStart -= GetIntent;
        StartEnemyState.OnEnterEnemyStart -= TurnStart;

    }

    private void GetIntent()
    {
        currentMove = moveSet.GetMove();
    }
    public void PerformAction()
    {
        switch (currentMove.MoveType)
        {
            case Move.Type.ATTACK:
                OnEnemyAttack?.Invoke(currentMove.Damage);
                break;
            case Move.Type.DEFEND:
                Defend(currentMove.ShieldAmt);
                break;
            case Move.Type.DEBUFF:
                Debug.Log("ENEMY : DEBUFF");
                break;
            case Move.Type.BUFF:
                Debug.Log("ENEMY : BUFF");
                break;
        }
        
    }

    private void Defend(float def) {
        currentDef += def;
        isShielded = true;
        Debug.Log("DEFEND : " + def);
    }

    private void CheckDeath()
    {
        if (currentHP <= 0)
        {
            //play death animation
        }
    }

    private void ShowFloatingText(string text)
    {
        GameObject floatText = Instantiate(FloatText, transform.position, Quaternion.identity, transform.Find("Canvas"));
        floatText.GetComponent<TMP_Text>().text = text;
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

        CheckDeath();
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
        ShowFloatingText(dmgTaken.ToString());
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
        ShowFloatingText(dmgTaken.ToString());
    }
    #endregion

    private void TurnStart()
    {
        if (isShielded)
        {
            currentDef = 0;
            isShielded = false;
        }
    }

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
