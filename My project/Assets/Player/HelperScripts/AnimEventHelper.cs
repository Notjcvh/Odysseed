using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventHelper : MonoBehaviour
{
  
        // Event calls

        // Player Animation Events --> Subscribers: Player Attack, Player Movement
        [SerializeField] private PlayerEvent TriggerEvent;
        [SerializeField] private PlayerEvent hasAttackAnimationEnded;
        [SerializeField] private PlayerEvent stopRotation;
        [SerializeField] private PlayerEvent hasJumpAnimationPlayed;
        [SerializeField] private PlayerEvent hasFallAnimationPlayed;
        [SerializeField] private PlayerEvent hasDashAnimationPlayed;
        [SerializeField] private PlayerEvent endWindUp;
        

        [SerializeField] private PlayerEvent callBehaviours;
        [SerializeField] private PlayerEvent finishBehaviours;
        


        public void AnimationTrigger()
        {
            TriggerEvent?.Raise();
        }

        public void AttackAnimationEndEvent()
        {
            hasAttackAnimationEnded?.Raise();
        }

        public void StopAnimationRotation()
        {
            stopRotation?.Raise();
        }
        
        public void JumpAnimationPlayed()
        { 
            hasJumpAnimationPlayed?.Raise();
        }
        
        public void FallingAnimationPlayed()
        {
            hasFallAnimationPlayed?.Raise();
        }

        public void DashAnimationPlayed()
        {
           hasDashAnimationPlayed?.Raise();
        }

        public void EndWindUp()
        {
           endWindUp?.Raise();
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
