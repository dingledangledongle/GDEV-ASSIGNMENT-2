using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    #region VARIABLES
    //DICE VARIABLES
    private int numOfDice = 6;

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

    //MISC
    private DamageType damage;
    private Animator animator;
    public GameObject FloatText;
    public GameObject SlashEffect;
    public AudioSource SlashSFX;
    public AudioSource BlockSFX;
    private EventManager eventManager = EventManager.Instance;
    #endregion

    private void Start()
    {
        currentHP = this.maxHP;
        currentDefValue = baseDefValue;
        animator = this.gameObject.GetComponent<Animator>();
        damage = new(baseDmg, baseNumberOfHits);

        #region Event Subscribing
        //BATTLE START
        eventManager.AddListener(Event.BATTLE_START, BattleStart);
        // TURN EVENTS
        eventManager.AddListener(Event.PLAYER_TURN, TurnStart);

        // ATTACK
        eventManager.AddListener<DamageType>(Event.PLAYER_ATTACK, GetDamage);
        eventManager.AddListener<int, bool>(Event.PLAYER_ATTACK, IsEnoughEnergy);
        eventManager.AddListener<int>(Event.PLAYER_ATTACK, ReduceCurrentEnergy);
        eventManager.AddListener(Event.PLAYER_ATTACK, PlayAttackAnim);
        eventManager.AddListener(Event.PLAYER_ATTACK_FINISHED, ResetDamageValues);

        //DEF
        eventManager.AddListener(Event.PLAYER_DEFEND, Defend);
        eventManager.AddListener(Event.PLAYER_DEFEND, PlayDefendAnim);
        eventManager.AddListener<int>(Event.PLAYER_DEFEND, ReduceCurrentEnergy);
        eventManager.AddListener<int, bool>(Event.PLAYER_DEFEND, IsEnoughEnergy);

        // MATERIAL ACTION
        eventManager.AddListener<float,int>(Event.PLAYER_ENHANCE_ATTACK, ModifyDamage);
        eventManager.AddListener<float>(Event.PLAYER_ENHANCE_DEFEND, ModifyDefense);
        eventManager.AddListener<int>(Event.PLAYER_ENHANCE_SUCCESS, ReduceCurrentEnergy);
        eventManager.AddListener<int, bool>(Event.PLAYER_ENHANCE, IsEnoughEnergy);

        //ENEMY EVENTS
        eventManager.AddListener<DamageType>(Event.ENEMY_ATTACK,TakeDamage);

        //DICE EVENTS
        eventManager.AddListener(Event.PLAYER_DICE, GetNumberOfDice);

        //REST EVENTS
        eventManager.AddListener<float>(Event.REST_HEAL, Heal);
        eventManager.AddListener(Event.REST_HEAL, GetMaxHP);
        eventManager.AddListener<float>(Event.REST_UPGRADEATTACK, UpgradeDamage);
        eventManager.AddListener<float>(Event.REST_UPGRADEDEFEND, UpgradeDefense);

        #region random events
        eventManager.AddListener<float>(Event.RAND_EVENT_UPGRADEATTACK, UpgradeDamage);
        eventManager.AddListener<float>(Event.RAND_EVENT_UPGRADEDEFEND, UpgradeDefense);
        eventManager.AddListener<float>(Event.RAND_EVENT_UPGRADEHEALTH, IncreaseMaxHealth);
        eventManager.AddListener<float>(Event.RAND_EVENT_HEAL, Heal);
        eventManager.AddListener<DamageType>(Event.RAND_EVENT_TAKEDAMAGE, TakeDamage);
        eventManager.AddListener<float>(Event.RAND_EVENT_REDUCEMAXHEALTH, IncreaseMaxHealth);
        #endregion


        #endregion

    }

    private void OnDestroy()
    {
        // TURN EVENTS
        eventManager.RemoveListener(Event.PLAYER_TURN, TurnStart);

        // ATTACK
        eventManager.RemoveListener<DamageType>(Event.PLAYER_ATTACK, GetDamage);
        eventManager.RemoveListener<int, bool>(Event.PLAYER_ATTACK, IsEnoughEnergy);
        eventManager.RemoveListener<int>(Event.PLAYER_ATTACK, ReduceCurrentEnergy);
        eventManager.RemoveListener(Event.PLAYER_ATTACK, PlayAttackAnim);
        eventManager.RemoveListener(Event.PLAYER_ATTACK_FINISHED, ResetDamageValues);

        //DEF
        eventManager.RemoveListener(Event.PLAYER_DEFEND, Defend);
        eventManager.RemoveListener(Event.PLAYER_DEFEND, PlayDefendAnim);
        eventManager.RemoveListener<int>(Event.PLAYER_DEFEND, ReduceCurrentEnergy);
        eventManager.RemoveListener<int, bool>(Event.PLAYER_DEFEND, IsEnoughEnergy);

        // MATERIAL ACTION
        eventManager.RemoveListener<float, int>(Event.PLAYER_ENHANCE_ATTACK, ModifyDamage);
        eventManager.RemoveListener<float>(Event.PLAYER_ENHANCE_DEFEND, ModifyDefense);
        eventManager.RemoveListener<int>(Event.PLAYER_ENHANCE_SUCCESS, ReduceCurrentEnergy);
        eventManager.RemoveListener<int, bool>(Event.PLAYER_ENHANCE, IsEnoughEnergy);

        //ENEMY EVENTS
        eventManager.RemoveListener<DamageType>(Event.ENEMY_ATTACK, TakeDamage);

        //DICE EVENTS
        eventManager.RemoveListener(Event.PLAYER_DICE, GetNumberOfDice);

        //REST EVENTS
        eventManager.RemoveListener<float>(Event.REST_HEAL, Heal);
        eventManager.RemoveListener(Event.REST_HEAL, GetMaxHP);
        eventManager.RemoveListener<float>(Event.REST_UPGRADEATTACK, UpgradeDamage);
        eventManager.RemoveListener<float>(Event.REST_UPGRADEDEFEND, UpgradeDefense);

        #region random events
        eventManager.RemoveListener<float>(Event.RAND_EVENT_UPGRADEATTACK, UpgradeDamage);
        eventManager.RemoveListener<float>(Event.RAND_EVENT_UPGRADEDEFEND, UpgradeDefense);
        eventManager.RemoveListener<float>(Event.RAND_EVENT_UPGRADEHEALTH, IncreaseMaxHealth);
        eventManager.RemoveListener<float>(Event.RAND_EVENT_HEAL, Heal);
        eventManager.RemoveListener<DamageType>(Event.RAND_EVENT_TAKEDAMAGE, TakeDamage);
        eventManager.RemoveListener<float>(Event.RAND_EVENT_REDUCEMAXHEALTH, IncreaseMaxHealth);
        #endregion
    }


    private float GetMaxHP()
    {
        return  maxHP;
    }

    private void Heal(float healAmt)
    {
        currentHP += healAmt;
        Debug.Log("healed for " + healAmt);
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

    private bool CheckPlayerDeath()
    {
        if (currentHP <= 0)
        {
            animator.Play("HeroKnight_Death");
            return true;
        }
        return false;
    }

    private bool IsEnoughEnergy(int cost)
    {
        if(currentEnergy < cost)
        {
            return false;
        }
        return true;
    }

    private int GetNumberOfDice()
    {
        return numOfDice;
    }

    #region VISUAL + SOUND FX   
    private void PlayAttackAnim()
    {
        animator.Play("HeroKnight_Attack1");
    }
    private void PlayDefendAnim()
    {
        animator.Play("HeroKnight_IdleBlock");
    }
    private void ShowFloatingText(string text)
    {
        Vector3 spawnPos = new(transform.position.x, transform.position.y + 10);
        GameObject floatText = Instantiate(FloatText, spawnPos, Quaternion.identity, transform.Find("Canvas"));
        floatText.GetComponent<TMP_Text>().text = text;
    }

    private void ShowSlashEffect()
    {
        Vector3 spawnPos = new(transform.position.x, transform.position.y + 10);
        SlashSFX.Play();
        Instantiate(SlashEffect, spawnPos, Quaternion.identity);
    }
    #endregion

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
        eventManager.TriggerEvent(Event.PLAYER_TAKEDAMAGE);
        if (CheckPlayerDeath())
        {
            //trigger player death
            SceneManager.LoadScene("Death");
        };
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
        if (currentDef <= 0)
        {
            isShielded = false;
        }
        BlockSFX.Play();

        return outstandingDmg;
    }

    private IEnumerator CalculateHealthDamage(DamageType damage)
    {
        for (int i = 0; i < damage.NumberOfHits; i++)
        {
            ReduceHealth(damage.DamagePerHit);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void ReduceHealth(float dmgTaken)
    {
        currentHP = Math.Max(currentHP - dmgTaken, 0);
        ShowFloatingText(dmgTaken.ToString());
        ShowSlashEffect();
    }
    #endregion
   
    #region MODIFYING VARIABLES
    private void ModifyDamage(float modifier,int numOfHits)
    {
        damage.NumberOfHits = numOfHits;
        damage.DamagePerHit += modifier;
    }

    private void ModifyDefense(float modifier)
    {
        currentDefValue += modifier;
    }

    private void UpgradeDefense(float defenseToAdd)
    {
        Debug.Log("defense upgraded : " + defenseToAdd);
        baseDefValue += defenseToAdd;
    }

    private void UpgradeDamage(float damageToAdd) {
        Debug.Log("damage upgraded : " + damageToAdd);

        baseDmg += damageToAdd;
    }

    private void UpgradeNumberOfHits(int hitsToAdd)
    {
        baseNumberOfHits += hitsToAdd;
    }

    private void IncreaseMaxHealth(float healthToAdd)
    {
        Debug.Log("max health increased " + healthToAdd);

        maxHP += healthToAdd;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    private void IncreaseMaxEnergy(int energyToAdd)
    {
        maxEnergy += energyToAdd;
    }

    private void IncreaseNumOfDice(int diceAdded)
    {
        numOfDice += diceAdded;
    }
    #endregion

    #region Turn Actions
    private void BattleStart()
    {
        //set materials to 0
        ResetDamageValues();

    }
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

    private void ResetDamageValues()
    {
        damage.DamagePerHit = baseDmg;
        damage.NumberOfHits = baseNumberOfHits;
    }

    #endregion

    #region GETTER / SETTER
    public int NumberOfDice
    {
        get
        {
            return numOfDice;
        }
        set
        {
            numOfDice = value;
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
