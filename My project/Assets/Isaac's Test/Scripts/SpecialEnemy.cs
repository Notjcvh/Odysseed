using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SpecialEnemy : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent navMeshAge;
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;
    public event System.Action<float> OnHealthPercentChange = delegate { };
    [Header("Combat")]
    public GameObject impactEffect;
    public bool isStunned;
    public float stunDuration;
    public float explodeRange;
    public float deaggroRange;
    public float aggroRange;
    private float distanceFromPlayer;
    public bool exploded;
    [Header("Movement")]
    public float idleDelay;
    private Transform currentWaypoint;
    public GameObject patrolPoint;
    public float movementSpeed;
    public float idleSpeed;

    private void Awake()
    {
        currentWaypoint = this.transform;
        currentHealth = maxHealth;
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
                TriggerExplode();
            }
        }
        else if(distanceFromPlayer < aggroRange)
        {
            //if the enemy sees the player but is not in attack range
            aggroRange = deaggroRange;
            navMeshAge.speed = movementSpeed;
            navMeshAge.destination = player.position;
        }
        else
        {
            //if enemy does not see the player do this
            navMeshAge.destination = currentWaypoint.position;
            navMeshAge.speed = idleSpeed;
        }
        if (currentHealth == 0)
        {
            Death();
        }
    }
    public void Death()
    {
        Destroy(this.gameObject);
    }
    public void ModifiyHealth(int amount)
    {
        currentHealth += amount;
        float currentHealthPercent = (float)currentHealth / (float)maxHealth;
        OnHealthPercentChange(currentHealthPercent);
    }

    public void TakeDamage(int damage)
    {
        this.currentHealth -= damage;
    }
    public void EnableAI()
    {
        this.navMeshAge.enabled = true;
        isStunned = false;
    }
    public void DisableAI()
    {
        this.navMeshAge.enabled = false;
        isStunned = true;
        Invoke("EnableAI", player.GetComponent<PlayerAttack>().knockbackTimer + stunDuration);
    }

    public void DisableAIforExplosion()
    {
        this.navMeshAge.enabled = false;
        Invoke("EnableAI", 3f);
    }
    public void TriggerExplode()
    {
        //Play explosion animation
        Invoke("CheckExplosionRange", 3f);
        DisableAIforExplosion();
    }

    public void CheckExplosionRange()
    {
        if (distanceFromPlayer <= explodeRange && !isStunned)
            Explode();
    }
    public void Explode()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);
        Destroy(gameObject);
        exploded = true;
    }

    IEnumerator FindRandomWaypoint()
    {
        yield return new WaitForSeconds(idleDelay);
        Vector3 newPatrolPoint = Random.insideUnitCircle * 5;
        GameObject newPatrolPointGO = Instantiate(patrolPoint, newPatrolPoint, transform.rotation);
        currentWaypoint = newPatrolPointGO.transform;
        Destroy(newPatrolPointGO, 3f);
        StartCoroutine(FindRandomWaypoint());
    }
}
