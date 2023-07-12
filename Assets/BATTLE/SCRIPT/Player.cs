using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public int currentEnergy;
    public int maxEnergy = 3;
   
    public void Reset()
    {
        dmg = 5;
    }
}
