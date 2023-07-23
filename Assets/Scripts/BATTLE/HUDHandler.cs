using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HUDHandler
{

    private void SetEnemyHUD(List<Enemy> enemyList)
    {
        foreach (Enemy enemy in enemyList)
        {
            //getting the components
            TMP_Text EnemyHPText = enemy.transform.Find("HealthBar/HealthNum").GetComponent<TMP_Text>();
            Image EnemyHPBar = enemy.transform.Find("HealthBar/Bar").GetComponent<Image>();

            //setting the values
            EnemyHPText.text = enemy.currentHP.ToString() + " / " + enemy.maxHP.ToString();
            EnemyHPBar.fillAmount = (float)(enemy.currentHP / enemy.maxHP);
            //EnemyDefText.text = enemy.def.ToString();
        }
    }

    private void SetPlayerHUD(Player player)
    {
        TMP_Text PlayerHPText = player.transform.Find("HealthBar/HealthNum").GetComponent<TMP_Text>();
        Image PlayerHPBar = player.transform.Find("HealthBar/Bar").GetComponent<Image>();
        TMP_Text PlayerDefText = player.transform.Find("ShieldBar/ShieldAmount").GetComponent<TMP_Text>();
        TMP_Text PlayerEnergyText = player.transform.Find("Energy/EnergyAmt").GetComponent<TMP_Text>();

        PlayerHPText.text = player.CurrentHP.ToString() + " / " + player.MaxHP.ToString();
        PlayerHPBar.fillAmount = (player.CurrentHP / player.MaxHP);

        if (player.IsShielded)
        {
            player.transform.Find("ShieldBar").gameObject.SetActive(true);
            PlayerDefText.text = player.CurrentDef.ToString();
        }
        else
        {
            player.transform.Find("ShieldBar").gameObject.SetActive(false);
        }

        PlayerEnergyText.text = player.CurrentEnergy.ToString() + "/" + player.MaxEnergy.ToString();
    }

    private void SetActionHUD(Player player)
    {
        GameObject ActionBar = GameObject.FindGameObjectWithTag("ActionBar");
        GameObject Attack = ActionBar.transform.Find("Attack").gameObject;
        GameObject Defend = ActionBar.transform.Find("Defend").gameObject;
        GameObject Inventory = ActionBar.transform.Find("Inventory").gameObject;
        GameObject Craft = ActionBar.transform.Find("Craft").gameObject;

        SetAttack(Attack, player);
        SetDefend(Defend, player);
    }

    private void SetAttack(GameObject attack, Player player)
    {
        TMP_Text dmgNum = attack.transform.Find("DamageText").GetComponent<TMP_Text>();
        if(player.NumberOfHits > 1)
        {
            dmgNum.text = player.CurrentDmg.ToString() + " x " + player.NumberOfHits.ToString();
        }
        else
        {
            dmgNum.text = player.CurrentDmg.ToString();
        }
       
    }

    private void SetDefend(GameObject defend, Player player)
    {
        TMP_Text defNum = defend.transform.Find("ShieldText").GetComponent<TMP_Text>();
        defNum.text = player.CurrentDefValue.ToString();
    }


    public void SetHUD(Player player, List<Enemy> enemyList)
    {
        SetEnemyHUD(enemyList);
        SetPlayerHUD(player);
        SetActionHUD(player);
    }
}
