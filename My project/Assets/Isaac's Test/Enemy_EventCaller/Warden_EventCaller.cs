using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warden_EventCaller : EnemyEventCaller
{
    private CarrotWarden _myBehavior;
    private bool processAnimationEvent = true;
    private AudioClip calledClip;

    private void Start()
    {
        _myBehavior = GetComponent<CarrotWarden>();
    }

    public override void AudioEventCalled(string audioType)
    {
        AudioType audio = AudioType.None;
        calledClip = null;
        if (audioType == "Attack")
        {
            int numberOfRandomNumbers = 4; // Number of random numbers to generate
            int minRange = 1; // Minimum value for random numbers
            int maxRange = 4;
            for (int i = 0; i < numberOfRandomNumbers; i++)
            {
                int randomNumber = Random.Range(minRange, maxRange + 1); // Generate a random number within the specified range
                switch (randomNumber)
                {
                    case 1:
                        audio = AudioType.Carrot_L_Attack1;
                        break;
                    case 2:
                        audio = AudioType.Carrot_L_Attack2;
                        break;
                    case 3:
                        audio = AudioType.Carrot_L_Attack3;
                        break;
                    case 4:
                        audio = AudioType.Carrot_L_Attack4;
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
                int randomNumber = Random.Range(minRange, maxRange + 1); // Generate a random number within the specified range
                switch (randomNumber)
                {
                    case 1:
                        audio = AudioType.Carrot_L_Damage1;
                        break;
                    case 2:
                        audio = AudioType.Carrot_L_Damage2;
                        break;
                    case 3:
                        audio = AudioType.Carrot_L_Damage3;
                        break;
                }
            }
            calledClip = _myBehavior.MyAudio[audio];
            _myBehavior.ManageAudio(audio);
            StartCoroutine(WaitToPlay(calledClip.length));
        }
        else if(audioType == "Death")
        {
            calledClip = _myBehavior.MyAudio[AudioType.Carrot_L_Death];
            _myBehavior.ManageAudio(AudioType.Carrot_L_Death);
            StartCoroutine(WaitToPlay(calledClip.length));
        }
    }
     
    IEnumerator WaitToPlay(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        processAnimationEvent = true;
    }
}
