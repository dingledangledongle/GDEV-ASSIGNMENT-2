using System.Collections.Generic;

public class ScorpionMoves : EnemyMoves
{
    private Dictionary<Move, float> MoveSet;
    private Move move1;
    private Move move2;
    private Move move3;
    private Move move4;

    private void Awake()
    {
        // MOVES
        move1 = new(Move.Type.ATTACK, 18f, 1);
        move2 = new(Move.Type.ATTACK, 10f, 2);
        move3 = new(Move.Type.DEFEND, 0f, 0, 15f);
        //move4 = new(Move.Type.DEBUFF);

        MoveSet = new()
        {
            { move1, 1.5f },
            { move2, 0.8f },
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
