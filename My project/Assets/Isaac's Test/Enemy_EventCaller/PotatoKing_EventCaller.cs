using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoKing_EventCaller : EnemyEventCaller
{
    private PotatoKing _myBehavior;
    private bool processAnimationEvent = true;
    private AudioClip calledClip;

    private void Start()
    {
        _myBehavior = GetComponent<PotatoKing>();
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
            if (audioType == "Damage")
            {
                int numberOfRandomNumbers = 3; // Number of random numbers to generate
                int minRange = 1; // Minimum value for random numbers
                int maxRange = 3;
                if (processAnimationEvent == true)
                {
                    for (int i = 0; i < numberOfRandomNumbers; i++)
                    {
                        int randomNumber = Random.Range(minRange, maxRange + 1);
                        switch (randomNumber)
                        {
                            case 1:
                                audio = AudioType.PK_DamageTaken;
                                break;
                            case 2:
                                audio = AudioType.PK_DamageTaken2;
                                break;
                            case 3:
                                audio = AudioType.PK_DamageTaken3;
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
            else if (audioType == "ComingUp")
            {
                calledClip = _myBehavior.MyAudio[AudioType.PK_ComingUp];
                _myBehavior.ManageAudio(AudioType.VineLordSpawnEnemy);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if(audioType == "GoingDown")
            {
                calledClip = _myBehavior.MyAudio[AudioType.PK_GoingDown];
                _myBehavior.ManageAudio(AudioType.PK_GoingDown);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if(audioType == "Spit")
            {
                calledClip = _myBehavior.MyAudio[AudioType.PK_Spit];
                _myBehavior.ManageAudio(AudioType.PK_Spit);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if (audioType == "Death")
            {
                calledClip = _myBehavior.MyAudio[AudioType.PK_Death];
                _myBehavior.ManageAudio(AudioType.PK_Death);
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
