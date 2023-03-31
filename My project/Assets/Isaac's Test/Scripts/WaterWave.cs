using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWave : Abilites
{
    public GameObject WaveAttack;
    public Transform abilitySpawner;
    public override void Ability()
    {
        Instantiate(WaveAttack, abilitySpawner);
        Debug.Log("water ability");
    }
}
