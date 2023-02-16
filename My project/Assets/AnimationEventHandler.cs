using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
   
    public GameEvent Event;
    
    void AnimationEventEnded()
    {
        Event?.Raise();
    }
}
