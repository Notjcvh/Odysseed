using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventHelper : MonoBehaviour
{
  
        // Event calls

        // Player Animation Events --> Subscribers: Player Attack 
        [SerializeField] private PlayerEvent TriggerEvent;
        [SerializeField] private PlayerEvent hasAnimationEnded;
        [SerializeField] private PlayerEvent stopRotation;


        [SerializeField] private PlayerEvent callBehaviours;
        [SerializeField] private PlayerEvent finishBehaviours;
        


        public void AnimationTrigger()
        {
            TriggerEvent?.Raise();
        }

        public void AnimationEndedEvent()
        {
            hasAnimationEnded?.Raise();
        }

        public void StopAnimationRotation()
        {
            stopRotation?.Raise();
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
