using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SpecialEnemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    private NavMeshAgent navMeshAge;

    public GameObject impactEffect;
    public float explodeRange;
    public float deaggroRange;
    public float aggroRange;
    private float distanceFromPlayer;

    public bool exploded;

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
        navMeshAge = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(FindRandomWaypoint());
    }

    private void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);
        if (distanceFromPlayer <= explodeRange)
        {
            transform.LookAt(player);
            navMeshAge.destination = player.position;
            if(!exploded)
            {
                Explode();
            }
        }
        else if(distanceFromPlayer < aggroRange)
        {
            //if the enemy sees the player but is not in attack range
            aggroRange = deaggroRange;
            navMeshAge.speed = 10;
            navMeshAge.destination = player.position;
        }
        else
        {
            //if enemy does not see the player do this
            navMeshAge.destination = currentWaypoint.position;
            navMeshAge.speed = 1;
        }

    }

    public void Explode()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);
        exploded = true;
    }

    IEnumerator FindRandomWaypoint()
    {
        yield return new WaitForSeconds(idleDelay);
        currentWaypoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        StartCoroutine(FindRandomWaypoint());
    }
}
