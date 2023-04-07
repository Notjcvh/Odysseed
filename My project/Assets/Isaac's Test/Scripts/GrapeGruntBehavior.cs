using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrapeGruntBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private NavMeshAgent navMeshAge;
    private AudioController audioController;

    [Header("Animation")]
    public Animator animator;

   
    [Header("Combat")]
    public float attackRange;
    public float attackSpeed;
    public float attackLife;
    public float attackPosChangeTimer;
    public bool isStunned = false;
    public Attack attackScript;
    private Transform currentAttackPos;
    private GameObject[] attackPoints;
    private int attackPointer = 0;
    public float aggroRange;
    public float deaggroRange;
    private float distanceFromPlayer;
    public GameObject attackHitbox;
    public float stunDuration = 10;
    public bool isTargeted;
    private float attackLifetime;
    private float attackCooldown;
    private Enemy thisEnemy;

    [Header("Movement")]
    public float movementSpeed;
    public float attackMoveSpeed;
    private float tempAttackMoveSpeed;
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
        navMeshAge = GetComponent<NavMeshAgent>();
        attackCooldown = attackSpeed;
        tempAttackMoveSpeed = attackMoveSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        attackPoints = GameObject.FindGameObjectsWithTag("AttackPoints");
        rb = this.GetComponent<Rigidbody>();
        currentAttackPos = player.transform;
        //currentAttackPos = attackPoints[0].GetComponent<Transform>();

        audioController = GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(audioController != null)
        {
            audioController.PlayAudio(AudioType.RotEnemyNoise, false, 0, false);
        }

        if (thisEnemy.isStunned)
        {
            DisableAI(stunDuration);
            thisEnemy.isStunned = false;
        }
        else
        {
            distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
     
            attackCooldown -= Time.deltaTime;
            attackLifetime -= Time.deltaTime;
            animator.SetFloat("Speed", rb.velocity.magnitude);
            if (attackPointer == 5)
            {
                attackPointer = 0;
            }
            if (attackLifetime <= 0)
            {
                attackHitbox.SetActive(false);
                animator.SetBool("IsAttacking", false);
                attackScript.hitAlready = false;
                tempAttackMoveSpeed = attackMoveSpeed;
            }
            if (distanceFromPlayer <= attackRange)
            {
                //if the enemy is in attack range do this
                navMeshAge.speed = tempAttackMoveSpeed;
                transform.LookAt(player.transform);
                navMeshAge.destination = currentAttackPos.position;
                if (attackCooldown <= 0)
                {
                    attackHitbox.SetActive(true);
                    animator.SetBool("IsAttacking", true);
                    tempAttackMoveSpeed = 0;
                    attackLifetime = attackLife;
                    attackCooldown = attackSpeed;
                }
            }
            else if (distanceFromPlayer < aggroRange)
            {
                //if the enemy sees the player but is not in attack range
                aggroRange = deaggroRange;
                navMeshAge.speed = tempAttackMoveSpeed;
                navMeshAge.destination = currentAttackPos.position;
            }
            else
            {
                //if enemy does not see the player do this
                navMeshAge.destination = currentWaypoint.position;
                navMeshAge.speed = idleSpeed;
            }
        }
    }

    public void DisableAI(float duration)
    {
        this.navMeshAge.enabled = false;
        attackCooldown = 99999f;
        isStunned = true;
        Invoke("EnableAI", Time.deltaTime * duration);
    }

    public void EnableAI()
    {
        attackCooldown = attackSpeed;
        isStunned = false;
        this.navMeshAge.enabled = true;
    }


    void PlayAudio(AudioType audioType)
    {
        audioController.PlayAudio(audioType, false, 0, false);
    }






}
