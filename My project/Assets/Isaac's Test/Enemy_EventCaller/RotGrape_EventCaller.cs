using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotGrape_EventCaller : EnemyEventCaller
{
    public GrapeGruntBehavior myBehavior;
    public bool processAnimationEvent = true;
    public override void AudioEventCalled(string audioType)
    {
        if(audioType == "Bark")
        {
            if (processAnimationEvent == true)
            {
                processAnimationEvent = false;
                myBehavior.ManageAudio(AudioType.RotEnemyNoise);
                //Wait to play 
                float delay = Random.Range(5, 10);
                StartCoroutine(WaitToPlay(delay));
            }
        }
        else if (audioType == "Death")
        {
           myBehavior.ManageAudio(AudioType.RotDeath);
        }
    }

    IEnumerator WaitToPlay(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        processAnimationEvent = true;
    }



}
