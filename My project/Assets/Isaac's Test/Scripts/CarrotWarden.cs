using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrotWarden : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    private NavMeshAgent age;
    private bool isMoving;
    private GameObject player;
    [Header("Attack attributes")]
    public int attackCounter = 0;
    public List<GameObject> attackHitboxes;
    private bool hasAttacked;
    private bool playerNearbyEndsAnimation;
    private bool playerInAttackRange;
    public float attackRange;
    private bool isAttacking = false;
    public LayerMask whatIsPlayer;
    public GameObject rangedProjectile;
    public Transform projectileSpawnLocation;
    public float projectileVelocity;
    public float projectileLifetime;
    [Header("LazerBeam")]
    private LazerBeam lb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        age = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!isAttacking)
        {
            switch (attackCounter)
            {
                case (0):
                    Idles();
                    break;
                case (1):
                    Walking();
                    break;
                case (2):
                    Attacking();
                    break;
                case (3):
                    Ranged();
                    break;
                default:
                    break;
            }
        }
        if (!isMoving)
        {
            age.SetDestination(player.transform.position);
            age.speed = 0f;
        }
        else
        {
            age.SetDestination(player.transform.position);
        }
        if (playerNearbyEndsAnimation)
        {
            if (playerInAttackRange)
            {
                Idle();
                playerNearbyEndsAnimation = false;
            }
        }
        Vector3 newTarget = player.transform.position;
        newTarget.y = 0;
        transform.LookAt(newTarget);
    }

    #region Walk
    public void Walking()
    {
        animator.SetBool("isWalking", true);
        age.speed = movementSpeed;
        playerNearbyEndsAnimation = true;
        isMoving = true;
        isAttacking = true;
    }
    #endregion

    #region Slashing
    public void Attacking()
    {
        Debug.Log("Im slashing");
        if(hasAttacked)
        {
            animator.SetBool("isAttacking", true);
            hasAttacked = false;
        }   
        else
        {
            animator.SetBool("isSlamming", true);
            hasAttacked = true;
        }
        isAttacking = true;
    }
    public void EnableAttackHitbox()
    {
        attackHitboxes[0].SetActive(true);
    }
    public void DIsableAttackHitbox()
    {
        attackHitboxes[0].SetActive(false);
    }
    #endregion

    #region Ranged
    public void Ranged()
    {
        animator.SetBool("isRanged", true);
        isAttacking = true;
    }
    public void SpawnRangedProjectile()
    {
        GameObject projectile = Instantiate(rangedProjectile, projectileSpawnLocation.position, transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileVelocity, ForceMode.Impulse);
        Destroy(projectile, projectileLifetime);
    }
    #endregion

    public void Death()
    {
        Debug.Log("Im Dying");
        isAttacking = true;
    }
    public void Idles()
    {
        animator.SetBool("isIdling", true);
        isAttacking = true;
    }
    public void EndChain()
    {
        attackCounter = 1;
        isAttacking = false;
        animator.SetBool("isIdling", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isSlamming", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isRanged", false);
    }
    public void endIdle()
    {
        animator.SetBool("isIdling", false);
        isAttacking = false;
    }
    public void Idle()
    {
        attackCounter += 1;
        isAttacking = false;
        isMoving = false;
        animator.SetBool("isIdling", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isSlamming", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isRanged", false);
    }
}
