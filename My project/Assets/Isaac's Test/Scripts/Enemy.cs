using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private NavMeshAgent navMeshAge;

    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;
    [Header("Combat")]
    public float stunDuration = 1f;
    public float attackRange;
    public float attackSpeed;
    public float attackLife;
    public float attackPosChangeTimer;
    private Transform currentAttackPos;
    private GameObject[] attackPoints;
    private int attackPointer = 0;
    public float aggroRange;
    public float deaggroRange;
    private float distanceFromPlayer;
    public GameObject attackHitbox;
    private float attackLifetime;
    private float attackCooldown;
    [Header("Movement")]
    public float movementSpeed;
    public float attackMoveSpeed;
    public float idleSpeed;
    public float idleDelay;
    public Transform currentWaypoint;
    public GameObject patrolPoint;

    [Header("Rooms")]
    public GameObject finishedRoom;
    public UnpackRoom locationInWorld;

    public event System.Action<float> OnHealthPercentChange = delegate { };

    private void Awake()
    {
        currentWaypoint = this.transform;
        currentHealth = maxHealth;
        navMeshAge = GetComponent<NavMeshAgent>();
        attackCooldown = attackSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        attackPoints = GameObject.FindGameObjectsWithTag("AttackPoints");
       
        currentAttackPos = attackPoints[0].GetComponent<Transform>();   
        StartCoroutine(FindRandomWaypoint());
        StartCoroutine(FindAttackWaypoint());
    }

    private void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
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
            navMeshAge.speed = attackMoveSpeed;
            transform.LookAt(player.transform);
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
            navMeshAge.speed = movementSpeed;
            navMeshAge.destination = currentAttackPos.position;
        }
        else
        {
            //if enemy does not see the player do this
            navMeshAge.destination = currentWaypoint.position;
            navMeshAge.speed = idleSpeed;
        }
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void WhichRoom(Collider colliderTrigger) 
    {
        GameObject location = colliderTrigger.gameObject;
        finishedRoom = location;
      
       
    }
    private void Death()
    {
        finishedRoom.GetComponent<UnpackRoom>().TransportEnemy(this.gameObject);
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

    public void DisableAI()
    {
        this.navMeshAge.enabled = false;
        Invoke("EnableAI", player.GetComponent<PlayerAttack>().knockbackTimer + stunDuration);
    }

    public void EnableAI()
    {
        this.navMeshAge.enabled = true;
    }
    IEnumerator FindRandomWaypoint()
    {
        yield return new WaitForSeconds(idleDelay);
        Vector3 newPatrolPoint = Random.insideUnitCircle * 5;
        GameObject newPatrolPointGO = Instantiate(patrolPoint, newPatrolPoint, transform.rotation);
        currentWaypoint = newPatrolPointGO.transform;
        Destroy(newPatrolPointGO, 5f);
        StartCoroutine(FindRandomWaypoint());
    }
    IEnumerator FindAttackWaypoint()
    {
        yield return new WaitForSeconds(attackPosChangeTimer);
        currentAttackPos = attackPoints[Random.Range(0, attackPoints.Length)].GetComponent<Transform>();
        StartCoroutine(FindAttackWaypoint());
    }
}
