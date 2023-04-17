using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Popper_EventCaller : EnemyEventCaller
{
    public SpecialEnemy popper;
    public override void SelectAudio(AudioType audioType)
    {
        if (calledAudio == false)
        {
            switch(audioType)
            {
                case AudioType.PopperDeath:

                    popper.ManageAudio(audioType);
                    StartCoroutine(WaitToPlay(delay));

                    break;
                case AudioType.PopperEnemyExplode:
                    popper.ManageAudio(audioType);
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
