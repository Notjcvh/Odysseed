using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthGrab : Abilites
{
    public GameObject earthPull;
    public Transform player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    public override void Ability()
    {
        GameObject earthPull = Instantiate(this.earthPull, player.position, player.rotation) as GameObject;
        Debug.Log("SAJHDGFBASJHDJASHDKAJSHDKJH");
    }
}
