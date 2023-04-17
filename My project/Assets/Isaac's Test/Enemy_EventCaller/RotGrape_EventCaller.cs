using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotGrape_EventCaller : EnemyEventCaller
{
    public Enemy enemy;
    public override void SelectAudio(AudioType audioType)
    { 
        if (calledAudio == false)
        {
            calledAudio = true;
            switch (audioType)
            {
                case AudioType.RotEnemyNoise:
                    delay = Random.Range(minDelay, maxDelay);
                    enemy.ManageAudio(audioType);
                    StartCoroutine(WaitToPlay(delay));
                    break;
                case AudioType.RotDeath:
                    enemy.ManageAudio(audioType);
                    calledAudio = false;
                    break;
            }          
        }
        else
        {
            return;
        }
       
    }

    public override IEnumerator WaitToPlay(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        calledAudio = false;
    }
}
