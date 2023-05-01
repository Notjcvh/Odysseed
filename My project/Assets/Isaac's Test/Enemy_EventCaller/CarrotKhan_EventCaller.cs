using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotKhan_EventCaller : EnemyEventCaller
{
    private carrotKhan _myBehavior;
    private bool processAnimationEvent = true;
    private AudioClip calledClip;

    private void Start()
    {
        _myBehavior = GetComponent<carrotKhan>();
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
            if(audioType == "Swing")
            {
                calledClip = _myBehavior.MyAudio[AudioType.CK_Swing];
                _myBehavior.ManageAudio(AudioType.CK_Swing);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if( audioType == "Slam")
            {
                calledClip = _myBehavior.MyAudio[AudioType.CK_Slam];
                _myBehavior.ManageAudio(AudioType.CK_Slam);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if(audioType == "Spin")
            {
                calledClip = _myBehavior.MyAudio[AudioType.CK_Spin];
                _myBehavior.ManageAudio(AudioType.CK_Spin);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if(audioType == "Punch")
            {
                calledClip = _myBehavior.MyAudio[AudioType.CK_Punch];
                _myBehavior.ManageAudio(AudioType.CK_Punch);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if(audioType == "SunBeam")
            {
                calledClip = _myBehavior.MyAudio[AudioType.CK_SunBeam];
                _myBehavior.ManageAudio(AudioType.CK_SunBeam);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if(audioType == "Phase1")
            {
                int numberOfRandomNumbers = 2; // Number of random numbers to generate
                int minRange = 1; // Minimum value for random numbers
                int maxRange = 2;
                for (int i = 0; i < numberOfRandomNumbers; i++)
                {
                    int randomNumber = Random.Range(minRange, maxRange + 1);
                    switch (randomNumber)
                    {
                        case 1:
                            audio = AudioType.CK_Phase1Quote;
                            break;
                        case 2:
                            audio = AudioType.CK_Phase1Quote2;
                            break;
                    }
                }
                _myBehavior.ManageAudio(audio);
                calledClip = _myBehavior.MyAudio[audio];
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if(audioType == "Phase2")
            {
                int numberOfRandomNumbers = 2; // Number of random numbers to generate
                int minRange = 1; // Minimum value for random numbers
                int maxRange = 2;
                for (int i = 0; i < numberOfRandomNumbers; i++)
                {
                    int randomNumber = Random.Range(minRange, maxRange + 1);
                    switch (randomNumber)
                    {
                        case 1:
                            audio = AudioType.CK_Phase2Quote;
                            break;
                        case 2:
                            audio = AudioType.CK_Phase2Quote2;
                            break;
                    }
                }
                _myBehavior.ManageAudio(audio);
                calledClip = _myBehavior.MyAudio[audio];

                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if(audioType == "Transfrom")
            {
                calledClip = _myBehavior.MyAudio[AudioType.CK_TransformationQuote];
                _myBehavior.ManageAudio(AudioType.CK_TransformationQuote);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if(audioType == "Death")
            {
                calledClip = _myBehavior.MyAudio[AudioType.CK_DeathQoute];
                _myBehavior.ManageAudio(AudioType.CK_DeathQoute);
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
