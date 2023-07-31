using System.Collections.Generic;

public class WispMoves : EnemyMoves
{
    private Dictionary<Move, float> MoveSet;
    private Move move1;
    private Move move2;
    private Move move3;
    private Move move4;

    private void Awake()
    {
        // MOVES
        move1 = new(Move.Type.ATTACK, 5f, 1);
        move2 = new(Move.Type.ATTACK, 2f, 2);
        move3 = new(Move.Type.DEFEND, 0f, 0, 10f);
        move4 = new(Move.Type.ATTACK, 2f, 5);
        //move4 = new(Move.Type.DEBUFF);

        //WEIGHTED PROBABILITY
        MoveSet = new()
        {
            { move1, 1f },
            { move2, 2f },
            { move3, 1f },
            { move4,0.5f}
        };
    }

    public override Move GetMove()
    {
        Move moveSelected = ProbabilityManager.SelectWeightedItem(MoveSet);
        //Debug.Log("MOVE SELECTED");
        return moveSelected;
    }
}
