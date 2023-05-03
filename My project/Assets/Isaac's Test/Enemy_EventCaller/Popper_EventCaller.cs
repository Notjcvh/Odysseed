using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Popper_EventCaller : EnemyEventCaller
{
    public PopperEnemyy _myBehavior;
    public bool processAnimationEvent = true;
    public AudioClip calledClip;

    private void Start()
    {
        _myBehavior = GetComponent<PopperEnemyy>();
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

            if(audioType == "Explode")
            {
                calledClip = _myBehavior.MyAudio[AudioType.PopperEnemyExplode];
                _myBehavior.ManageAudio(AudioType.PopperEnemyExplode);
                StartCoroutine(WaitToPlay(calledClip.length));
            }
            else if(audioType == "Death")
            {
                calledClip = _myBehavior.MyAudio[AudioType.PopperDeath];
                _myBehavior.ManageAudio(AudioType.PopperDeath);
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
