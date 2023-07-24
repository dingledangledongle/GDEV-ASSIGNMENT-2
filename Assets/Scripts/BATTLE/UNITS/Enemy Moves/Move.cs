using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public enum Type
    {
        ATTACK,
        DEFEND,
        DEBUFF,
        BUFF
    }

    private DamageType damage;
    private float damagePerHit;
    private int numOfHits;
    private float shieldAmt;
    public Type MoveType;
    //add status class

    public Move(Type type,float dmgPerHit = 0,int numHits = 0 ,float shieldValue=0)
    {
        MoveType = type;
        shieldAmt = shieldValue;
        damagePerHit = dmgPerHit;
        numOfHits = numHits;
        test();
    }
    private void test()
    {
        damage = new(damagePerHit, numOfHits);

    }
    public DamageType Damage
    {
        get
        {
            return damage;
        }
    }
    public float ShieldAmt
    {
        get
        {
            return shieldAmt;
        }
    }

    public float DamageNum
    {
        get
        {
            return damage.DamagePerHit;
        }
    }

    public int NumHit
    {
        get
        {
            return damage.NumberOfHits;
        }
    }
}
