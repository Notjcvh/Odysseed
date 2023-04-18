using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotGrape_EventCaller : EnemyEventCaller
{
    public Enemy enemy;
    public bool processAnimationEvent = true;
    public override void AudioEventCalled(string audioType)
    {
        if(audioType == "Bark")
        {
            if (processAnimationEvent == true)
            {
                processAnimationEvent = false;
                enemy.ManageAudio(AudioType.RotEnemyNoise);
                //Wait to play 
                float delay = Random.Range(5, 10);
                StartCoroutine(WaitToPlay(delay));
            }
        }
        else if (audioType == "Death")
        {
           enemy.ManageAudio(AudioType.RotDeath);
        }
    }

    IEnumerator WaitToPlay(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        processAnimationEvent = true;
    }



}
