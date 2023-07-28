using System;
using UnityEngine;
using TMPro;
using System.Collections;


public class Enemy : MonoBehaviour
{
    public float maxHP;
    private float currentHP;
    private float currentDef;
    private bool isShielded = false;
    private bool isTargetable = true;
    private bool isDead = false;

    
    private EnemyMoves moveSet;
    private Move currentMove;
    public AudioSource defendSFX;
    public GameObject FloatText;
    private Animator animator;

    //events
    public static event Action<DamageType> OnEnemyAttack;

    private void Awake()
    {
        currentHP = maxHP;
        moveSet = this.gameObject.GetComponent<EnemyMoves>();
        animator = this.gameObject.GetComponent<Animator>();

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
    private void PlayAttackAnimation()
    {
        animator.Play("Attack");
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
                PlayAttackAnimation();
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
        defendSFX.Play();
        animator.Play("Defend");
    }

    private void CheckDeath()
    {
        if (currentHP <= 0)
        {
            //play death animation
            isTargetable = false;
            isDead = true;
        }
    }

    private void ShowFloatingText(string text)
    {
        Vector3 spawnPos = new(transform.position.x, transform.position.y + 10);
        GameObject floatText = Instantiate(FloatText,spawnPos, Quaternion.identity, transform.Find("Canvas"));
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
            StartCoroutine(CalculateHealthDamage(damage));
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

    public bool IsTargetable
    {
        get
        {
            return isTargetable;
        }
    }
    public bool IsDead
    {
        get
        {
            return isDead;
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
