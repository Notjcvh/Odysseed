using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot_EventCaller : EnemyEventCaller
{
    private CarrotGrunt _myBehavior;

    private bool processAnimationEvent = true;
    private AudioClip calledClip;

    private void Start()
    {
        _myBehavior = GetComponent<CarrotGrunt>();
    }


    public override void AudioEventCalled(string audioType)
    {
        if (_myBehavior.audioTableSet != true)
        {
            return;
        }
        else
        {
            AudioType audio = AudioType.None;
            calledClip = null;
            if (audioType == "Attack")
            {
                int numberOfRandomNumbers = 3; // Number of random numbers to generate
                int minRange = 1; // Minimum value for random numbers
                int maxRange = 3;
                for (int i = 0; i < numberOfRandomNumbers; i++)
                {
                    int randomNumber = Random.Range(minRange, maxRange + 1); // Generate a random number within the specified range
                    switch (randomNumber)
                    {
                        case 1:
                            audio = AudioType.CarrotAttack1;
                            break;
                        case 2:
                            audio = AudioType.CarrotAttack2;
                            break;
                        case 3:
                            audio = AudioType.CarrotAttack3;
                            break;
                    }
                }
                calledClip = _myBehavior.MyAudio[audio];
                _myBehavior.ManageAudio(audio);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if (audioType == "Damage")
            {
                int numberOfRandomNumbers = 3; // Number of random numbers to generate
                int minRange = 1; // Minimum value for random numbers
                int maxRange = 3;
                for (int i = 0; i < numberOfRandomNumbers; i++)
                {
                    int randomNumber = Random.Range(minRange, maxRange + 1);
                    switch (randomNumber)
                    {

                        case 1:
                            audio = AudioType.CarrotDamage1;
                            break;
                        case 2:
                            audio = AudioType.CarrotDamage2;
                            break;
                        case 3:
                            audio = AudioType.CarrotDamage3;
                            break;
                    }
                }
                calledClip = _myBehavior.MyAudio[audio];
                _myBehavior.ManageAudio(audio);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if (audioType == "Swing")
            {
                calledClip = _myBehavior.MyAudio[AudioType.CarrotSwing];
                _myBehavior.ManageAudio(AudioType.CarrotSwing);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if (audioType == "Death")
            {
                int numberOfRandomNumbers = 3; // Number of random numbers to generate
                int minRange = 1; // Minimum value for random numbers
                int maxRange = 3;
                for (int i = 0; i < numberOfRandomNumbers; i++)
                {
                    int randomNumber = Random.Range(minRange, maxRange + 1);
                    switch (randomNumber)
                    {
                        case 1:
                            audio = AudioType.CarrotDeath1;
                            break;
                        case 2:
                            audio = AudioType.CarrotDeath2;
                            break;
                        case 3:
                            audio = AudioType.CarrotDeath3;
                            break;
                    }
                    calledClip = _myBehavior.MyAudio[audio];
                    _myBehavior.ManageAudio(audio);
                    StartCoroutine(WaitToPlay(calledClip.length));
                }
            }
        }
    }

    IEnumerator WaitToPlay(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        processAnimationEvent = true;
    }

}
