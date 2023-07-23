using UnityEngine;

public abstract class PlayerMaterial : MonoBehaviour
{
    private int amount;
    private float damageModifier;
    private float defenseModifier;
    private int numOfHits;
    private bool isAOE = false;

    public virtual int Amount
    {
        get
        {
            return amount;
        }
        set
        {
            amount = value;
        }
    }

    public virtual float DamageModifier
    {
        get
        {
            return damageModifier;
        }
        set
        {
            damageModifier = value;
        }
    }

    public virtual float DefenseModifier
    {
        get
        {
            return defenseModifier;
        }
        set
        {
            defenseModifier = value;
        }
    }

    public virtual int NumOfHits
    {
        get
        {
            return numOfHits;
        }
        set
        {
            numOfHits = value;
        }
    }

    public virtual bool IsAOE
    {
        get
        {
            return isAOE;
        }
        set
        {
            isAOE = value;
        }
    }
}
