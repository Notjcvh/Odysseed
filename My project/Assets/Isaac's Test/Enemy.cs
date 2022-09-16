using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    private NavMeshAgent navMeshAge;

    public float attackRange;
    public float sightRange;
    public float distanceFromPlayer;

    public float idleDelay;
    public Transform currentWaypoint;
    public Transform[] patrolPoints;

    private void Awake()
    {
        navMeshAge = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(FindRandomWaypoint());
    }

    private void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);

        
        if(distanceFromPlayer < attackRange)
        {
            //if the enemy is in attack range do this
            navMeshAge.destination = this.transform.position;

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
