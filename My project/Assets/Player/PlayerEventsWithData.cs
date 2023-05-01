using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerEventWithData", menuName = "Events/Player Event With Data")]

public class PlayerEventsWithData : PlayerEvent
{
    [TextArea]
    public string text;
    public override void Raise()
    {
        base.Raise();
    }
}
