using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rest : MonoBehaviour
{
    private float healPercentage = 0.3f;

    public static event Action<float> OnRestPressed;
    public static event Func<float> OnMaxHPGet;

    public static event Action<float> OnUpgradeAttack;
    public static event Action<float> OnUpgradeDefend;


    public void HealPlayer()
    {
        float maxHP = OnMaxHPGet.Invoke();
        float healthToBeHealed = maxHP * healPercentage;

        OnRestPressed?.Invoke(healthToBeHealed);
    }

    public void UpgradeAttack()
    {
        OnUpgradeAttack?.Invoke(1f);
    }

    public void UpgradeDefense()
    {
        OnUpgradeDefend?.Invoke(1f);
    }
    
}
