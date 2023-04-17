using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventHelper : MonoBehaviour
{
        public PlayerManger playerManger;

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

        [SerializeField] private PlayerEvent Death;



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

        public void PlayerDied()
        {
          Death?.Raise();
        }

        public void CallBehaviours()
        {
            callBehaviours?.Raise();
        }

        public void FinisheBehaviours()
        {
            finishBehaviours?.Raise();
        }


    public void SelectAudio(string type)
    {
        //PlayerAttack
        //ChargedAttack


        if (type == "PlayerWalk")
        {
            AudioType sendingAudio = AudioType.None;
            int numberOfRandomNumbers = 5; // Number of random numbers to generate
            int minRange = 1; // Minimum value for random numbers
            int maxRange = 5;
            for (int i = 0; i < numberOfRandomNumbers; i++)
            {
                int randomNumber = Random.Range(minRange, maxRange + 1); // Generate a random number within the specified range
                switch (randomNumber)
                {
                    case (1):
                        sendingAudio = AudioType.PlayerWalk1;
                        break;
                    case (2):
                        sendingAudio = AudioType.PlayerWalk2;
                        break;
                    case (3):
                        sendingAudio = AudioType.PlayerWalk3;
                        break;
                    case (4):
                        sendingAudio = AudioType.PlayerWalk4;
                        break;
                    case (5):
                        sendingAudio = AudioType.PlayerWalk5;
                        break;
                    default:
                        break;
                }
            }
            playerManger.ManageAudio(sendingAudio);
        }
        else if(type == "PlayerDeath")
        {
            AudioType sendingAudio = AudioType.None;
            sendingAudio = AudioType.PlayerDeath;
            playerManger.ManageAudio(sendingAudio);
        }
        else
        {
            return;
        }
    }
}
