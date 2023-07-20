using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleHud : MonoBehaviour
{
    public TMP_Text PlayerHPText;
    public TMP_Text PlayerDefText;
    public TMP_Text PlayerEnergyText;
    public TMP_Text EnemyHPText;
    public TMP_Text EnemyDefText;
   
    public void setEnemyHUD(Unit unit) {
        EnemyHPText.text = "HP : " + unit.currentHP.ToString() + " / " + unit.maxHP.ToString();
        EnemyDefText.text = "DEF : " + unit.def.ToString();

    }

    public void setPlayerHUD(Player unit)
    {
        PlayerHPText.text = "HP : " + unit.currentHP.ToString() + " / " + unit.maxHP.ToString();
        PlayerDefText.text = "DEF : " + unit.def.ToString();
        PlayerEnergyText.text = "Energy : " + unit.currentEnergy.ToString() + " / " + unit.maxEnergy.ToString();
    }

    public void setHUD(Player player, Unit enemy)
    {
        setEnemyHUD(enemy);
        setPlayerHUD(player);
    }
}
