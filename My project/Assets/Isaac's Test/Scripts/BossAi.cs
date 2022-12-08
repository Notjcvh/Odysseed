using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAi : MonoBehaviour
{
    [Header("Generic Attributes")]
    public float genericAtkSpeed;
    private float genericAtkSpeedCounter;
    public int currentHealth;
    public int maxHealth;
    [Header("Movement")]
    public float movementSpeed;
    [Header("Ranged Attributes")]
    public float attack1Speed;
    public GameObject attack1;
    public float attack1Life;
    [Header("Melee Attributes")]
    public float meleeCooldown;
    private float meleeCooldownCounter;
    public GameObject spawnLocation;
    public GameObject attack2;
    public Attack attackScript;
    public bool activateMelee;
    public float attack2Life;
    [Header("Summon Enemy Attributes")]
    public float summonEnemyCooldown;
    private float summonEnemyCounter;
    public GameObject projectileSpawnLocation;
    public GameObject projectile;
    public GameObject enemy;

    private GameObject player;
    public float distanceFromPlayer;
    private NavMeshAgent navMeshAge;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAge = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        genericAtkSpeedCounter -= Time.deltaTime;
        meleeCooldownCounter -= Time.deltaTime;
        summonEnemyCounter -= Time.deltaTime;
        navMeshAge.destination = player.transform.position;
        if (genericAtkSpeedCounter <= 0)
        {
            Attack1();
            genericAtkSpeedCounter = genericAtkSpeed;
        }
        if (meleeCooldownCounter <= 0 && activateMelee)
        {
            navMeshAge.speed = 0;
            activateMelee = false;
            meleeCooldownCounter = meleeCooldown;
            //activate animation add event to animation that spawns collider & reenableMovespeed;
        }
        if(summonEnemyCounter <= summonEnemyCooldown)
        {
            //activate animation with scripts to summon enemy;
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
        Vector3 spawnLocation = this.spawnLocation.transform.position;
        SpawnAttack2(spawnLocation);
    }

    public void TakeDamage(int damage)
    {
        this.currentHealth -= damage;
    }

    public void SpawnAttack1()
    {
        Vector3 spawnLocation = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z);
        GameObject inGameAttack1 = Instantiate(attack1, spawnLocation, player.transform.rotation);
        Destroy(inGameAttack1, attack1Life);
    }
    public void SpawnAttack2(Vector3 spawnLocation)
    {
        GameObject inGameAttack1 = Instantiate(attack2, spawnLocation, this.transform.rotation);
        Destroy(inGameAttack1, attack2Life);
    }
    public void SummonEnemy()
    {
        GameObject inGameAttack1 = Instantiate(projectile, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
    }

    
}
