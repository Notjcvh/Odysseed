using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : MonoBehaviour
{
    [Header("Generic Attributes")]
    public float genericAtkSpeed;
    private float genericAtkSpeedCounter;
    public int bossHealth;
    public int meleeAtkRange;
    public int nextAttack;
    [Header("Attack1 Attributes")]
    public float attack1Speed;
    public GameObject attack1;
    public float attack1Life;
    [Header("Attack2 Attributes")]
    public float attack2Speed;
    public GameObject[] spawnLocations;
    public GameObject attack2;
    public float attack2Life;
    [Header("Summon Enemy Attributes")]
    public float summonEnemySpeed;
    public GameObject enemy;

    private GameObject player;
    public float distanceFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        genericAtkSpeedCounter -= Time.deltaTime;
        if(genericAtkSpeedCounter <= 0)
        {
            if (distanceFromPlayer >= meleeAtkRange)
            {
                if (nextAttack != 1)
                    nextAttack = 1;
                else
                    nextAttack = 3;
            }
            else
            {
                if (nextAttack != 2)
                    nextAttack = 2;
                else
                    nextAttack = 3;
            }
            if(nextAttack == 1)
            {
                Attack1();
            }
            else if (nextAttack == 2)
            {
                Attack2();
            }
            else
            {
                SummonEnemyAtack();
            }
            genericAtkSpeedCounter = genericAtkSpeed;
        }
    }

    public void Attack1()
    {
        Invoke("SpawnAttack1", attack1Speed);
        Invoke("SpawnAttack1", attack1Speed*2);
        Invoke("SpawnAttack1", attack1Speed*3);
    }

    public void Attack2()
    {
        foreach (var item in spawnLocations)
        {
            Vector3 spawnLocation = item.transform.position;
            SpawnAttack2(spawnLocation);
        }
        
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
    public void SpawnAttack2(Vector3 spawnLocation)
    {
        GameObject inGameAttack1 = Instantiate(attack1, spawnLocation, player.transform.rotation);
        Destroy(inGameAttack1, attack2Life);
    }
    public void SummonEnemy()
    {
        GameObject inGameAttack1 = Instantiate(enemy, player.transform.position, player.transform.rotation);
    }

    
}
