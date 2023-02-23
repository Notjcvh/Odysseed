using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AnimationStateHandler : MonoBehaviour
{
    // Triggers
    public GameEvent TriggerEvent; 
    public GameEvent EndEvent;

    protected Animator animator;

    void AnimationTrigger()
    {
        TriggerEvent?.Raise();
    }

    void AnimationEventEnded()
    {
        EndEvent?.Raise();
    }
}





