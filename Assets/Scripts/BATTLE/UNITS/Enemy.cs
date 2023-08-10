using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Enemy : MonoBehaviour
{
    public float maxHP;
    private float currentHP;
    private float currentDef;
    private bool isShielded = false;
    private bool isTargetable = true;
    private bool isDead = false;
    private bool isFinished = false;

    
    private EnemyMoves moveSet;
    private Move currentMove;
    public AudioSource defendSFX;
    public GameObject FloatText;
    private Animator animator;
    private EventManager eventManager = EventManager.Instance;

    private void Awake()
    {
        currentHP = maxHP;
        moveSet = gameObject.GetComponent<EnemyMoves>();
        animator = gameObject.GetComponent<Animator>();

        //EVENTS
        eventManager.AddListener(Event.PLAYER_TURN,GetIntent);
        eventManager.AddListener(Event.PLAYER_ATTACK_FINISHED, CheckDeath);
        Debug.Log(gameObject.name + " INITIALIZED");
    }

    private void OnDestroy()
    {
        eventManager.RemoveListener(Event.PLAYER_TURN, GetIntent);
        eventManager.RemoveListener(Event.PLAYER_ATTACK_FINISHED, CheckDeath);
    }

    private void Heal(float healAmt)
    {
        currentHP += healAmt;
        //prevent enemy from exceeding max hp
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void TurnStart()
    {
        //resets the enemy shield on the start of their turn
        if (isShielded)
        {
            currentDef = 0;
            isShielded = false;
        }
    }

    #region Visual FX / Animations
    private void PlayAttackAnimation()
    {
        animator.Play("Attack");
    }
    private void ShowFloatingText(string text)
    {
        //spawns a floating text to display damage on the enemy
        Vector3 spawnPos = new(transform.position.x, transform.position.y + 10);
        GameObject floatText = Instantiate(FloatText, spawnPos, Quaternion.identity, transform.Find("Canvas"));
        floatText.GetComponent<TMP_Text>().text = text;
    }

    #endregion

    private void GetIntent()
    {
        //get the current intent of the enemy
        currentMove = moveSet.GetMove();
    }

    public void PerformAction()
    {
        // plays out the appropriate actions according to the intent of the enemy
        switch (currentMove.MoveType)
        {
            case Move.Type.ATTACK:
                eventManager.TriggerEvent(Event.ENEMY_ATTACK, currentMove.Damage);
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
        eventManager.TriggerEvent(Event.UPDATE_HUD);
    }

    private void Defend(float def) {
        currentDef += def;
        isShielded = true;
        defendSFX.Play();
        animator.Play("Defend");
    }

    private void CheckDeath()
    {
        Debug.Log("checking death...");
        if (currentHP <= 0)
        {
            //play death animation
            isTargetable = false;
            isDead = true;
            animator.Play("Death");
        }

        //check all other enemy is dead
        bool isAllEnemyDead = eventManager.TriggerEvent<bool>(Event.ENEMY_DEATH);
        Node.Encounter encounterType = eventManager.TriggerEvent<Node.Encounter>(Event.ENEMY_DEATH);
        if (isAllEnemyDead)
        {
            if (encounterType == Node.Encounter.BOSS)
            {
                Debug.Log("victory");
                SceneManager.LoadScene("Victory");
            }
            Debug.Log("all dead");
            eventManager.TriggerEvent(Event.ENEMY_DEATH);
        }
    }

    #region Damage Calculation
    //DAMAGE CALCULATION PART
    public void TakeDamage(DamageType damage)
    {
        //if the enemy is shielded, the shield damage calculation occurs first
        //any outstanding damage that the shield couldn't block would be dealt to the enemy's health
        if (isShielded)
        {
            float outstandingDmg = CalculateShieldDamage(damage);
            if (outstandingDmg > 0)
            {
                ReduceHealth(outstandingDmg);
            }
            isFinished = true;
        }
        else
        {
            StartCoroutine(CalculateHealthDamage(damage));
        }
    }

    private float CalculateShieldDamage(DamageType damage)
    {
        float outstandingDmg = 0;// set the counter for amount of outstanding damage

        for (int i = 0; i < damage.NumberOfHits; i++)
        {
            //adds outstanding damage if any
            outstandingDmg += ReduceShield(damage.DamagePerHit); 
        }

        //return the outstanding damage to be processed by ReduceHealth
        if (outstandingDmg > 0)
        {
            return outstandingDmg;
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
            yield return new WaitForSeconds(0.1f);// add a delay so the floating text and sound effects wouldn't stack together
        }

        isFinished = true; // indicate that enemy finished taking damage
    }
    private void ReduceHealth(float dmgTaken)
    {
        currentHP = Math.Max(currentHP - dmgTaken, 0);
        ShowFloatingText(dmgTaken.ToString());
    }

    public bool IsDamageCalculationDone()
    {
        //check if damage calculation is completed before the enemy can end their turn
        return isFinished;
    }
    #endregion

    #region GETTER / SETTER

    public bool IsFinished
    {
        get
        {
            return isFinished;
        }
        set
        {
            isFinished = value;
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
