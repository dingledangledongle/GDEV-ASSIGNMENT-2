using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTheWheel : RandomEvent
{
    private RandomEvents eventName = RandomEvents.SpinTheWheel;
    private string title = "Spin The Wheel!";
    private string body = "In the depths of the forest, a malevolent devil materializes, offering a sinister proposition.\n" +
        "A mystic wheel, with rewards and punishments entwined, awaits your decision.\n" +
        "Will you dare to spin, risking the unpredictable whims of fate?\n" +
        "Choose wisely, Hephaestus, for the forest holds its breath, anticipating the outcome.";
    private string option1_text = "[Spin] You spin the wheel...";
    private string option2_text = "[Leave] You ignore the devil...";

    public override RandomEvents EventName {
        get => eventName; 
    }

    #region GETTER/SETTER
    public override string Title { get => title;  }

    public override string Body { get => body; }

    public override string Option1_Text { get => option1_text;  }

    public override string Option2_Text { get => option2_text;  }
    #endregion

    public override void Option_1() {
        //activate    
    }

    public override void Option_2() { 
        //end event
    }
}
