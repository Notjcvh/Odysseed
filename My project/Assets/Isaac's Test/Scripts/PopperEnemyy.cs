using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopperEnemyy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private UnityEngine.AI.NavMeshAgent navMeshAge;

    [Header("Animation")]
    public Animator animator;

    [Header("Combat")]
    public float attackRange;
    public bool isStunned = false;
    public float aggroRange;
    public float deaggroRange;
    private float distanceFromPlayer;
    public GameObject attackHitbox;
    public float stunDuration = 10;
    public bool isTargeted;
    private Enemy thisEnemy;

    [Header("Movement")]
    public float movementSpeed;
    public float idleSpeed;
    public float idleDelay;
    public Transform currentWaypoint;
    public GameObject patrolPoint;
    public Rigidbody rb;
    // Start is called before the first frame update


    void Awake()
    {
        currentWaypoint = this.transform;
        thisEnemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        navMeshAge = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = this.GetComponent<Rigidbody>();
        //currentAttackPos = attackPoints[0].GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAge.speed = movementSpeed;
        if (thisEnemy.isStunned)
        {
            DisableAI(stunDuration);
            thisEnemy.isStunned = false;
        }
        else
        {
            distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
            
            if (distanceFromPlayer <= attackRange)
            {
                //if the enemy is in attack range do this
                transform.LookAt(player.transform);
                movementSpeed = 0;
                animator.SetBool("isRunning", false);
                TriggerExplosion();
            }
            else if (distanceFromPlayer < aggroRange)
            {
                //if the enemy sees the player but is not in attack range
                navMeshAge.destination = player.transform.position;
                animator.SetBool("isRunning", true);
            }
            else
            {
                //if enemy does not see the player do this
                animator.SetBool("isRunning", true);
                navMeshAge.destination = player.transform.position;
            }
            if(distanceFromPlayer > deaggroRange)
            {
                animator.SetBool("isRunning", false);
                navMeshAge.destination = this.transform.position;
            }
        }
    }

    public void TriggerExplosion()
    {
        animator.SetBool("isExploding", true);
    }
    public void Explode()
    {
        if(distanceFromPlayer < attackRange)
        {
            Instantiate(attackHitbox, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else
        {
            animator.SetBool("isExploding", false);
            navMeshAge.destination = player.transform.position;
            movementSpeed = 10;
        }
    }
    public void DisableAI(float duration)
    {
        this.navMeshAge.enabled = false;
        isStunned = true;
        Invoke("EnableAI", Time.deltaTime * duration);
    }

    public void EnableAI()
    {
        isStunned = false;
        this.navMeshAge.enabled = true;
    }
}
