using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AnimationStateHandler : MonoBehaviour
{
    // Triggers
    public PlayerEvent TriggerEvent; 
    public PlayerEvent EndEvent;
    public PlayerEvent canKnockUp;
    public PlayerEvent canKnockBack;

    protected Animator animator;

    void AnimationTrigger()
    {
        TriggerEvent?.Raise();
    }

    void AnimationEventEnded()
    {
        EndEvent?.Raise();
    }

    void CanKnockUp()
    {
        canKnockUp?.Raise();
    }

    void CanKnockBack()
    {
        canKnockBack?.Raise();
    }



}





