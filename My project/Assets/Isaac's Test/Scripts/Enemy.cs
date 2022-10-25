using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    private NavMeshAgent navMeshAge;

    public int maxHealth = 3;
    public int currentHealth;

    public float attackRange;
    public float attackSpeed;
    public float attackLife;
    public Transform currentAttackPos;
    public GameObject[] attackPoints;
    public float attackPosChangeTimer;
    public int attackPointer = 0;

    public float deaggroRange;
    public float aggroRange;
    private float distanceFromPlayer;

    public GameObject attackHitbox;
    private float attackLifetime;
    private float attackCooldown;

    public float idleDelay;
    public Transform currentWaypoint;
    public Transform[] patrolPoints;

    private void Awake()
    {
        if (patrolPoints.Length != 0)
        {
            currentWaypoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        }
        else
        {
            currentWaypoint = this.transform;
        }
        currentHealth = maxHealth;
        navMeshAge = GetComponent<NavMeshAgent>();
        attackCooldown = attackSpeed;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        attackPoints = GameObject.FindGameObjectsWithTag("AttackPoints");
        currentAttackPos = attackPoints[0].GetComponent<Transform>();
        StartCoroutine(FindRandomWaypoint());
        StartCoroutine(FindAttackWaypoint());
    }

    private void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);
        attackCooldown -= Time.deltaTime;
        attackLifetime -= Time.deltaTime;
        if (attackPointer == 5)
        {
            attackPointer = 0;
        }
        if (attackLifetime <= 0)
        {
            attackHitbox.SetActive(false);
        }
        if (distanceFromPlayer <= attackRange)
        {
            //if the enemy is in attack range do this
            navMeshAge.speed = 3;
            transform.LookAt(player);
            navMeshAge.destination = currentAttackPos.position;
            if(attackCooldown <= 0)
            {
                attackHitbox.SetActive(true);
                attackLifetime = attackLife;
                attackCooldown = attackSpeed;
            }
        }
        else if(distanceFromPlayer < aggroRange)
        {
            //if the enemy sees the player but is not in attack range
            aggroRange = deaggroRange;
            navMeshAge.speed = 10;
            navMeshAge.destination = currentAttackPos.position;
        }
        else
        {
            //if enemy does not see the player do this
            navMeshAge.destination = currentWaypoint.position;
            navMeshAge.speed = 1;
        }
        if(currentHealth == 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
    public void Stun(float stunDuration)
    {
        Debug.Log("Stun for" + stunDuration);
    }
    IEnumerator FindRandomWaypoint()
    {
        yield return new WaitForSeconds(idleDelay);
        currentWaypoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        StartCoroutine(FindRandomWaypoint());
    }
    IEnumerator FindAttackWaypoint()
    {
        yield return new WaitForSeconds(attackPosChangeTimer);
        currentAttackPos = attackPoints[Random.Range(0, attackPoints.Length)].GetComponent<Transform>();
        StartCoroutine(FindAttackWaypoint());
    }
}
