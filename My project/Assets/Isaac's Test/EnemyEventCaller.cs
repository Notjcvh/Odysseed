using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class EnemyEventCaller : MonoBehaviour
{
    public AudioType[] type1;

    protected float delay;
    public float maxDelay = .5f;
    public float minDelay = 10f;
    public bool calledAudio;

    public abstract void SelectAudio(AudioType audioType);
    public abstract IEnumerator WaitToPlay(float time);
   
   
}
