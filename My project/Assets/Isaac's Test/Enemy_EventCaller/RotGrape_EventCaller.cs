using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotGrape_EventCaller : EnemyEventCaller
{
    public Enemy enemy;
    public override void AudioEventCalled(string audioType)
    {
        enemy.SelectAudio(audioType);  
    }
}
