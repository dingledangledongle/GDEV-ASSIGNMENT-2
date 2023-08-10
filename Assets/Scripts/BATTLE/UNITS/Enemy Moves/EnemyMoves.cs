using UnityEngine;

public abstract class EnemyMoves : MonoBehaviour
{
    //abstract class that different enemies' move inherit from
    public abstract Move GetMove();
    
}
