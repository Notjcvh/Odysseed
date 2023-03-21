using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventHelper : MonoBehaviour
{
  
        // Triggers
        [SerializeField] private PlayerEvents TriggerEvent;
        [SerializeField] private PlayerEvents EndAnimationEvent;
        [SerializeField] private PlayerEvents callBehaviours;
        [SerializeField] private PlayerEvents finishBehaviours;

        protected Animator animator;

        public void AnimationTrigger()
        {
            TriggerEvent?.Raise();
        }

        public void AnimationEventEnded()
        {
            EndAnimationEvent?.Raise();
        }

        public void CallBehaviours()
        {
            callBehaviours?.Raise();
        }

        public void FinisheBehaviours()
        {
            finishBehaviours?.Raise();
        }
}
