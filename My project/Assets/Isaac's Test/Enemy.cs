using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    private NavMeshAgent navMeshAge;

    public float attackRange;
    public float attackSpeed;
    public float attackLife;
    
    public GameObject attackHitbox;
    private float attackLifetime;
    private float attackCooldown;
    public bool isAttacking;
    public float sightRange;
    public float distanceFromPlayer;

    public float idleDelay;
    public Transform currentWaypoint;
    public Transform[] patrolPoints;

    private void Awake()
    {
        navMeshAge = GetComponent<NavMeshAgent>();
        isAttacking = false;
        attackCooldown = attackSpeed;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(FindRandomWaypoint());
    }

    private void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);

        
        if(distanceFromPlayer <= attackRange)
        {
            //if the enemy is in attack range do this
            navMeshAge.destination = this.transform.position;
            attackCooldown -= Time.deltaTime;
            if(isAttacking)
            {
                attackLifetime -= Time.deltaTime;
                if(attackLifetime <= 0)
                {
                    isAttacking = false;
                    attackHitbox.SetActive(false);
                }
            }
            if(attackCooldown <= 0)
            {
                attackHitbox.SetActive(true);
                isAttacking = true;
                attackLifetime = attackLife;
                attackCooldown = attackSpeed;
            }
        }
        else if(distanceFromPlayer < sightRange)
        {
            //if the enemy sees the player but is not in attack range
            navMeshAge.destination = player.position;
        }
        else
        {
            //if enemy does not see the player do this
            navMeshAge.destination = currentWaypoint.position;
        }

    }

    IEnumerator FindRandomWaypoint()
    {
        yield return new WaitForSeconds(idleDelay);
        currentWaypoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        StartCoroutine(FindRandomWaypoint());
    }
}
