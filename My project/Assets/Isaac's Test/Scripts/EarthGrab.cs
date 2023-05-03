using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthGrab : Abilites
{
    public GameObject earthPull;
    public Transform player;
    public Transform abilitySpawner;
    public float abilityDuration;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        abilityCooldown = GameObject.FindGameObjectWithTag("EarthSeed").GetComponent<AbilityCooldown>();
    }
    public override void Ability()
    {
        GameObject earthPull = Instantiate(this.earthPull,new Vector3(abilitySpawner.position.x, abilitySpawner.position.y - 1.28f, abilitySpawner.position.z), player.rotation) as GameObject;
        Destroy(earthPull, abilityDuration);
    }
}
