using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDHandler
{
    public TMP_Text PlayerHPText;
    public TMP_Text PlayerDefText;
    public TMP_Text PlayerEnergyText;
    public TMP_Text EnemyHPText;
    public TMP_Text EnemyDefText;
   
    private void setEnemyHUD(Enemy[] enemyList) {
        foreach (Enemy enemy in enemyList)
        {
            TMP_Text EnemyHP = enemy.transform.Find("HealthBar/HealthNum").GetComponent<TMP_Text>();
            EnemyHP.text = enemy.currentHP.ToString() + " / " + enemy.maxHP.ToString();
            //EnemyDefText.text = enemy.def.ToString();
        }
       
    }

    private void setPlayerHUD(Player player)
    {
        TMP_Text PlayerHP = player.transform.Find("HealthBar/HealthNum").GetComponent<TMP_Text>();
        PlayerHP.text = "HP : " + player.currentHP.ToString() + " / " + player.maxHP.ToString();
        //PlayerDefText.text = "DEF : " + player.def.ToString();
        //PlayerEnergyText.text = "Energy : " + player.currentEnergy.ToString() + " / " + player.maxEnergy.ToString();
    }

    public void setHUD(Player player, Enemy[] enemyList)
    {
        setEnemyHUD(enemyList);
        setPlayerHUD(player);
    }
}
