using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : MonoBehaviour
{
    [Header("Attack1 Attributes")]
    public float attack1Speed;
    public GameObject attack1;
    public float minDistance;
    public bool isAttack1ing;
    private int atkCounter;


    private GameObject player;
    public float distanceFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Attack1();
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
    }

    public void Attack1()
    {
        Invoke("SpawnAttack", attack1Speed);
        Invoke("SpawnAttack", attack1Speed*2);
        Invoke("SpawnAttack", attack1Speed*3);
    }

    public void SpawnAttack()
    {
        Instantiate(attack1, player.transform.position, player.transform.rotation);
    }
}
