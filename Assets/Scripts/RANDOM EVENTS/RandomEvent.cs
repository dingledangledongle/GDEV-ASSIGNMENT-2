using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RandomEvent
{
    public abstract RandomEvents EventName {get;}
    public abstract string Title { get;}
    public abstract string Body { get;}
    public abstract string Option1_Text { get; }
    public abstract string Option2_Text { get; }

    public abstract void Option_1();

    public abstract void Option_2();
}

public enum RandomEvents
{
    SpinTheWheel,
    FreeUpgrade,
    event3
}
