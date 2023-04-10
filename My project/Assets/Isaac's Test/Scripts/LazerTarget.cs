using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerTarget : MonoBehaviour
{
    public GameObject player;
    public float groundlevel = 0f;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, speed);
    }
}
