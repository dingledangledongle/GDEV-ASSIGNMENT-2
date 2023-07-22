using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDHandler
{
    public TMP_Text PlayerDefText;
    public TMP_Text PlayerEnergyText;
    public TMP_Text EnemyDefText;
   
    private void SetEnemyHUD(List<Enemy> enemyList) {
        foreach (Enemy enemy in enemyList)
        {
            //getting the components
            TMP_Text EnemyHPText = enemy.transform.Find("HealthBar/HealthNum").GetComponent<TMP_Text>();
            Image EnemyHPBar = enemy.transform.Find("HealthBar/Bar").GetComponent<Image>();

            //setting the values
            EnemyHPText.text = enemy.currentHP.ToString() + " / " + enemy.maxHP.ToString();
            EnemyHPBar.fillAmount = (float) (enemy.currentHP / enemy.maxHP);
            //EnemyDefText.text = enemy.def.ToString();
        }
    }

    private void SetPlayerHUD(Player player)
    {
        TMP_Text PlayerHPText = player.transform.Find("HealthBar/HealthNum").GetComponent<TMP_Text>();
        Image PlayerHPBar = player.transform.Find("HealthBar/Bar").GetComponent<Image>();
        TMP_Text PlayerDefText = player.transform.Find("ShieldBar/ShieldAmount").GetComponent<TMP_Text>();
        TMP_Text PlayerEnergyText = player.transform.Find("Energy/EnergyAmt").GetComponent<TMP_Text>();

        PlayerHPText.text = player.currentHP.ToString() + " / " + player.maxHP.ToString();
        PlayerHPBar.fillAmount = (player.currentHP / player.maxHP);

        if (player.isShielded)
        {
            player.transform.Find("ShieldBar").gameObject.SetActive(true);
            PlayerDefText.text = player.currentDef.ToString();
        }
        else
        {
            player.transform.Find("ShieldBar").gameObject.SetActive(false);
        }
        
        PlayerEnergyText.text =player.currentEnergy.ToString() + "/" + player.maxEnergy.ToString();
    }


    public void SetHUD(Player player, List<Enemy> enemyList)
    {
        SetEnemyHUD(enemyList);
        SetPlayerHUD(player);
    }
}
