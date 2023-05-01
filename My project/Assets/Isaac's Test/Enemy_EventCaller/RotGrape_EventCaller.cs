using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotGrape_EventCaller : EnemyEventCaller
{
    private GrapeGruntBehavior _myBehavior;
    public bool processAnimationEvent = true;
    public AudioClip calledClip;

    private void Start()
    {
        _myBehavior = GetComponent<GrapeGruntBehavior>();
    }
    public override void AudioEventCalled(string audioType)
    {
        if(_myBehavior.audioTableSet != true)
        {
            return;
        }
        else
        {
            AudioType audio = AudioType.None;
            calledClip = null;
            if (audioType == "Bark")
            {
                int numberOfRandomNumbers = 4; // Number of random numbers to generate
                int minRange = 1; // Minimum value for random numbers
                int maxRange = 4;
                if (processAnimationEvent == true)
                {
                    for (int i = 0; i < numberOfRandomNumbers; i++)
                    {
                        int randomNumber = Random.Range(minRange, maxRange + 1);
                        switch (randomNumber)
                        {
                            case 1:
                                audio = AudioType.RotGrowl_1;
                                break;
                            case 2:
                                audio = AudioType.RotGrowl_2;
                                break;
                            case 3:
                                audio = AudioType.RotGrowl_3;
                                break;
                            case 4:
                                audio = AudioType.RotGrowl_4;
                                break;
                            case 5:
                                audio = AudioType.RotEnemyNoise;
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
