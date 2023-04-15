using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventCaller : MonoBehaviour
{
    public Enemy enemy;
    public AudioType[] type1;

    private float delay;
    public float maxDelay = .5f;
    public float minDelay = 10f;
    public bool calledAudio;

    public void Play(AudioType audioType)
    {
       if(calledAudio == false)
       {
            calledAudio = true;
            delay = Random.Range(minDelay, maxDelay);
            foreach (var audioTypeElements in type1)
            {
                if (audioType == audioTypeElements)
                {
                    enemy.ManageAudio(audioType);
                    StartCoroutine(WaitToPlay(delay));
                }
            }
       }
      
    }

    IEnumerator WaitToPlay(float time)
    {
    
        yield return new WaitForSecondsRealtime(time);
        calledAudio = false;
    }
}
