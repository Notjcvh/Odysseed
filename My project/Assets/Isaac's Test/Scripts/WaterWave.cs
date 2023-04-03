using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWave : Abilites
{
    public GameObject waveAttack;
    public Transform abilitySpawner;
    public override void Ability()
    {
        GameObject wave = Instantiate(waveAttack,abilitySpawner,true) as GameObject;
        Debug.Log("water ability");
    }
}
