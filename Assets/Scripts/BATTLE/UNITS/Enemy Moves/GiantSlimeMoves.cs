using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSlimeMoves : EnemyMoves
{
    private Dictionary<Move, float> MoveSet;
    private Move move1;
    private Move move2;
    private Move move3;
    private Move move4;

    private void Awake()
    {
        // MOVES
        move1 = new(Move.Type.ATTACK, 5f, 6);
        move2 = new(Move.Type.ATTACK, 3f, 8);
        move3 = new(Move.Type.DEFEND, 0f, 0, 10f);
        move4 = new(Move.Type.DEBUFF);

        MoveSet = new()
        {
            { move1, 0.6f },
            { move2, 1.5f },
            { move3, 1f }
        };
    }

    public override Move GetMove()
    {
        Move moveSelected = ProbabilityManager.SelectWeightedItem(MoveSet);
        //Debug.Log("MOVE SELECTED");
        return moveSelected;
    }
}
