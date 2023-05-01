using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotLord_EventCaller : EnemyEventCaller
{
    private BossAi _myBehavior;
    private bool processAnimationEvent = true;
    private AudioClip calledClip;

    private void Start()
    {
        _myBehavior = GetComponent<BossAi>();
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
            if (audioType == "Bark")
            {
                int numberOfRandomNumbers = 5; // Number of random numbers to generate
                int minRange = 1; // Minimum value for random numbers
                int maxRange = 5;
                if (processAnimationEvent == true)
                {
                    for (int i = 0; i < numberOfRandomNumbers; i++)
                    {
                        int randomNumber = Random.Range(minRange, maxRange + 1);
                        switch (randomNumber)
                        {
                            case 1:
                                audio = AudioType.VineLordGrowl_1;
                                break;
                            case 2:
                                audio = AudioType.VineLordGrowl_2;
                                break;
                            case 3:
                                audio = AudioType.VineLordGrowl_3;
                                break;
                            case 4:
                                audio = AudioType.VineLordGrowl_4;
                                break;
                            case 5:
                                audio = AudioType.VineLordGrowl_5;
                                break;
                        }
                    }
                    processAnimationEvent = false;
                    _myBehavior.ManageAudio(audio);

                    calledClip = _myBehavior.MyAudio[audio];
                    float delay = Random.Range(5, 10);
                    StartCoroutine(WaitToPlay(calledClip.length + delay));
                }
            }
            else if(audioType == "Toss")
            {
                calledClip = _myBehavior.MyAudio[AudioType.VineLordSpawnEnemy];
                _myBehavior.ManageAudio(AudioType.VineLordSpawnEnemy);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if (audioType == "Death")
            {
                calledClip = _myBehavior.MyAudio[AudioType.RotDeath];
                _myBehavior.ManageAudio(AudioType.RotDeath);
                StartCoroutine(WaitToPlay(calledClip.length));

            }
        }
    }

    IEnumerator WaitToPlay(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        processAnimationEvent = true;
    }


}
