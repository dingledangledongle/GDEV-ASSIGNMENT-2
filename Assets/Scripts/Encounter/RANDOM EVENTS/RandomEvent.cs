using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RandomEvent :MonoBehaviour
{
    public abstract RandomEvents EventName {get;}


    public abstract void Option_1();

    public abstract void Option_2();
}

public enum RandomEvents
{
    SpinTheWheel,
    FreeUpgrade,
    ReachInDepth
}
