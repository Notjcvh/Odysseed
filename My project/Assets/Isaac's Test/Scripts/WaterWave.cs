using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWave : Abilites
{
    public GameObject waveAttack;
    public GameObject player;
    public Transform abilitySpawner;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Ability()
    {
        GameObject wave = Instantiate(waveAttack, abilitySpawner.position, player.transform.rotation) as GameObject;
    }
}
