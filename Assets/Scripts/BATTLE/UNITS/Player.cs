using System;
using System.Collections;
using TMPro;
using UnityEngine;

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

    public static event Action OnPlayerDamageTaken; //BattleManager.UpdateHud()
    private void Start()
    {
        currentHP = this.maxHP;
        currentDefValue = baseDefValue;
        animator = this.gameObject.GetComponent<Animator>();
        damage = new(baseDmg,baseNumberOfHits);

        #region Event Subscribing
        // TURN EVENTS
        StartPlayerState.OnPlayerStart += TurnStart;
        EndPlayerState.OnPlayerEnd += TurnEnd;

        // ATTACK
        eventManager.AddListener<DamageType>(Event.PLAYER_ATTACK, GetDamage);
        eventManager.AddListener<int, bool>(Event.PLAYER_ATTACK,IsEnoughEnergy);
        eventManager.AddListener<int>(Event.PLAYER_ATTACK, ReduceCurrentEnergy);
        eventManager.AddListener(Event.PLAYER_ATTACK, PlayAttackAnim);
        eventManager.AddListener(Event.PLAYER_ATTACK_FINISHED, ResetDamageValues);

        //DEF
        DefendAction.OnDefend += Defend;
        DefendAction.OnDefend += PlayDefendAnim;
        DefendAction.OnAfterDef += ReduceCurrentEnergy;
        DefendAction.BeforeDefend += IsEnoughEnergy;

        // MATERIAL ACTION
        MaterialAction.OnAttackEnhance += ModifyDamage;
        MaterialAction.OnDefEnhance += ModifyDefense;
        MaterialAction.OnSuccessEnhance += ReduceCurrentEnergy;
        MaterialAction.BeforeAction += IsEnoughEnergy;

        //ENEMY EVENTS
        Enemy.OnEnemyAttack += TakeDamage;

        //DICE EVENTS
        DiceHandler.OnDiceBoxSpawn += GetNumberOfDice;

        //REST EVENTS
        eventManager.AddListener<float>(Event.REST_HEAL, Heal);
        eventManager.AddListener<float>(Event.REST_HEAL, GetMaxHP);
        eventManager.AddListener<float>(Event.REST_UPGRADEATTACK, UpgradeDamage);
        eventManager.AddListener<float>(Event.REST_UPGRADEDEFEND, UpgradeDefense);

        #region spin the wheel events
        eventManager.AddListener<float>(Event.RAND_EVENT_STW_UPGRADEATTACK, UpgradeDamage);
        eventManager.AddListener<float>(Event.RAND_EVENT_STW_UPGRADEDEFEND, UpgradeDefense);
        eventManager.AddListener<float>(Event.RAND_EVENT_STW_UPGRADEHEALTH, IncreaseMaxHealth);
        eventManager.AddListener<float>(Event.RAND_EVENT_STW_HEAL, Heal);
        eventManager.AddListener<DamageType>(Event.RAND_EVENT_STW_TAKEDAMAGE, TakeDamage);
        eventManager.AddListener<float>(Event.RAND_EVENT_STW_REDUCEMAXHEALTH, IncreaseMaxHealth);
        #endregion



        #endregion

    }

    private void OnDestroy()
    {
        // TURN EVENTS
        StartPlayerState.OnPlayerStart -= TurnStart;
        EndPlayerState.OnPlayerEnd -= TurnEnd;

        // ATTACK
        eventManager.RemoveListener<DamageType>(Event.PLAYER_ATTACK, GetDamage);
        eventManager.RemoveListener<int, bool>(Event.PLAYER_ATTACK, IsEnoughEnergy);
        eventManager.RemoveListener<int>(Event.PLAYER_ATTACK, ReduceCurrentEnergy);
        eventManager.RemoveListener(Event.PLAYER_ATTACK, PlayAttackAnim);
        eventManager.RemoveListener(Event.PLAYER_ATTACK_FINISHED, ResetDamageValues);
        eventManager.RemoveListener<float>(Event.RAND_EVENT_STW_UPGRADEATTACK, UpgradeDamage);
        eventManager.RemoveListener<float>(Event.RAND_EVENT_STW_UPGRADEDEFEND, UpgradeDefense);
        eventManager.RemoveListener<float>(Event.RAND_EVENT_STW_UPGRADEHEALTH, IncreaseMaxHealth);

        //DEF
        DefendAction.OnDefend -= Defend;
        DefendAction.OnDefend-= PlayDefendAnim;
        DefendAction.OnAfterDef -= ReduceCurrentEnergy;
        DefendAction.BeforeDefend -= IsEnoughEnergy;


        // MATERIAL ACTION
        MaterialAction.OnAttackEnhance -= ModifyDamage;
        MaterialAction.OnDefEnhance -= ModifyDefense;
        MaterialAction.OnSuccessEnhance -= ReduceCurrentEnergy;
        MaterialAction.BeforeAction -= IsEnoughEnergy;


        //ENEMY EVENTS
        Enemy.OnEnemyAttack -= TakeDamage;

        //DICE EVENTS
        DiceHandler.OnDiceBoxSpawn -= GetNumberOfDice;

        //REST EVENTS
        eventManager.RemoveListener<float>(Event.REST_HEAL, Heal);
        eventManager.RemoveListener<float>(Event.REST_HEAL, GetMaxHP);
        eventManager.RemoveListener<float>(Event.REST_UPGRADEATTACK, UpgradeDamage);
        eventManager.RemoveListener<float>(Event.REST_UPGRADEDEFEND, UpgradeDefense);
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
    private void TurnStart()
    {
        currentEnergy = maxEnergy;
        if (isShielded)
        {
            currentDef = 0;
            isShielded = false;
        }
        ResetDamageValues();

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
