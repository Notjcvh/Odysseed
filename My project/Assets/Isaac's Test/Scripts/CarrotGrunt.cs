using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrotGrunt : MonoBehaviour
{
    [Header("Movement Attributes")]
    public float baseMoveSpeed;
    private float moveSpeed;
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    [Header("Attack Attributes")]
    public float attackRange, sightRange;
    public bool playerInSightRange, playerinAttackRange;
    public bool isAttacking;
    public GameObject attackHitbox;

    public LayerMask whatIsGround, whatIsPlayer;
    private Animator animator;
    private NavMeshAgent nav;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nav = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        moveSpeed = baseMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerinAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        nav.speed = moveSpeed;
        BasicState();
    }

    public void BasicState()
    {
        if (!playerInSightRange && !playerinAttackRange)
        {
            Walking();
        }
        else if (playerInSightRange && !playerinAttackRange)
        {
            ChasePlayer();
        }
        else if (playerinAttackRange && playerInSightRange)
        {
            Attack();
        }
    }

    public void Attack()
    {
        isAttacking = true;
        nav.SetDestination(this.transform.position);
        Vector3 newTarget = player.transform.position;
        newTarget.y = 0;
        transform.LookAt(newTarget);
        animator.SetBool("isAttacking", true);
        Debug.Log("isAttacking");
    }

    public void ChasePlayer()
    {
        nav.SetDestination(player.transform.position);
    }

    public void Walking()
    {
        moveSpeed = baseMoveSpeed;
        animator.SetBool("isWalking", true);
        if(!walkPointSet)
        {
            SearchWalkPoint();
        }
        if(walkPointSet)
        {
            nav.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    public void ActivateAttack()
    {
        attackHitbox.SetActive(true);
    }
    public void DeActivateAttack()
    {
        attackHitbox.SetActive(false);
    }
}
