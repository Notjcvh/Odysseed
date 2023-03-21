using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AnimationStateHandler : MonoBehaviour
{
    // Triggers
    public PlayerEvents TriggerEvent; 
    public PlayerEvents EndEvent;
    public PlayerEvents canKnockUp;
    public PlayerEvents canKnockBack;

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





