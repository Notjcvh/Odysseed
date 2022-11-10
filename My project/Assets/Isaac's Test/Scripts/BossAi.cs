using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : MonoBehaviour
{
    [Header("Attack1 Attributes")]
    public float attack1Speed;
    public GameObject attack1;
    public float attack1Life;
    [Header("Summon Enemy Attributes")]
    public float summonEnemySpeed;
    public GameObject enemy;

    private GameObject player;
    public float distanceFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Attack1();
        SummonEnemyAtack();
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
    }

    public void Attack1()
    {
        Invoke("SpawnAttack1", attack1Speed);
        Invoke("SpawnAttack1", attack1Speed*2);
        Invoke("SpawnAttack1", attack1Speed*3);
    }

    public void SummonEnemyAtack()
    {
        Invoke("SummonEnemy", attack1Speed);
        Invoke("SummonEnemy", attack1Speed * 2);
        Invoke("SummonEnemy", attack1Speed * 3);
    }

    public void SpawnAttack1()
    {
        Vector3 spawnLocation = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z);
        GameObject inGameAttack1 = Instantiate(attack1, spawnLocation, player.transform.rotation);
        Destroy(inGameAttack1, attack1Life);
    }

    public void SummonEnemy()
    {
        GameObject inGameAttack1 = Instantiate(enemy, player.transform.position, player.transform.rotation);
    }
}
