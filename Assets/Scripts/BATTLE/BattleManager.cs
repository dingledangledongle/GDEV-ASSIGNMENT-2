using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
    {
        STATE_START,
        STATE_PLAYERTURN,
        STATE_END,
        STATE_ENEMYTURN,
        STATE_ENEMYEND
    }

public class BattleManager : MonoBehaviour
{
    [SerializeField] State state;
    [SerializeField] BattleHud HUD;
    [SerializeField] Player player;
    [SerializeField] Enemy enemy;


    void setupBattle()
    {
        player.currentEnergy = player.maxEnergy;
        player.currentHP = player.maxHP;
        enemy.currentHP = enemy.maxHP;
        //START FIRST TURN STUFF

        HUD.setHUD(player, enemy);
        playerTurn();
    }

    void updateHUD()
    {
        HUD.setHUD(player, enemy);
    }


    void playerTurn()
    {
        //roll stuff
        //start turn effects

        state = State.STATE_PLAYERTURN;
        player.currentEnergy = player.maxEnergy;
        player.def = 0;
        updateHUD();

    }

    public void AtkBtn()
    {
        if(state != State.STATE_PLAYERTURN)
            return;
        if (player.currentEnergy == 0)
            return;

        player.Attack(enemy);
        player.currentEnergy -= 1;
        updateHUD();
    }

    public void EndTurnBtn()
    {
        endTurn();
    }

    void endTurn()
    {
        state = State.STATE_END;
        // do all the end turn effects

        enemyMove();
    }

    void enemyMove()
    {
        state = State.STATE_ENEMYTURN;
        enemy.def = 0;
        string intent = enemy.GenerateIntent();
        switch (intent)
        {
            case "attack":
                enemy.Attack(enemy,player);
                break;
            case "defend":
                enemy.Defend(enemy);
                break;
        }
        updateHUD();

        enemyEnd();
    }

    void enemyEnd()
    {
        state = State.STATE_ENEMYEND;
        //do enemy turn end stuff

        playerTurn();
    }

    void Start()
    {
        state = State.STATE_START;
        setupBattle();
    }
}
