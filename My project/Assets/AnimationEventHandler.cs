using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

    public GameEvent TriggerEvent; 
    public GameEvent EndEvent;

    void AnimationTrigger()
    {
        TriggerEvent?.Raise();
    }

    void AnimationEventEnded()
    {
        EndEvent?.Raise();
    }
}
