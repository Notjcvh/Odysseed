using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossLevel : MonoBehaviour
{
    public GameObject player;
    public Transform startingPosition;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = startingPosition.position;
    }
}
