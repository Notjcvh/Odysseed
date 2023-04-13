using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class carrotKhan : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float spinSpeed;
    public float chargeSpeed;
    private NavMeshAgent age;
    private bool isMoving;
    private GameObject player;
    [Header("Attack attributes")]
    public int attackCounter = 0;
    public List<GameObject> attackHitboxes;
    private bool playerNearbyEndsAnimation;
    private bool playerInAttackRange;
    public float attackRange;
    private bool isAttacking = false;
    public int healthThreshold = 50;
    private Enemy enemyScript;
    public LayerMask whatIsPlayer;
    [Header("Phase 2")]
    public bool hasExploded = false;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        enemyScript = this.GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player");
        age = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!isAttacking && !hasExploded)
        {
            switch(attackCounter)
            {
                case (0):
                    Idles();
                    break;
                case (1):
                    Taunt();
                    break;
                case (2):
                    Run();
                    break;
                case (3):
                    Slashing();
                    break;
                case (4):
                    Whirlwind();
                    break;
                case (5):
                    OverHeadSwing();
                    break;
                default:
                    break;
            }
        }
        if(!hasExploded && enemyScript.currentHealth < healthThreshold)
        {
            Explode();
        }
        if (!isAttacking && hasExploded)
        {
            switch (attackCounter)
            {
                case(0):
                    Taunt();
                    break;
                case (1):
                    Whirlwind();
                    break;
                case (2):
                    Charge();
                    break;
                case (3):
                    Slashing();
                    break;
                default:
                    break;
            }
        }
        if(!isMoving)
        {
            age.SetDestination(player.transform.position);
            age.speed = 0.1f;
        }
        else
        {
            age.SetDestination(player.transform.position);
        }
        if(playerNearbyEndsAnimation)
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
    #region Taunt
    public void Taunt()
    {
        Debug.Log("Im taunting");
        animator.SetBool("isTaunting", true);
        isAttacking = true;
    }
    #endregion

    #region Run
    public void Run()
    {
        Debug.Log("Im running");
        animator.SetBool("isRunning", true);
        age.speed = movementSpeed;
        playerNearbyEndsAnimation = true;
        isMoving = true;
        isAttacking = true;
    }
    #endregion

    #region Slashing
    public void Slashing()
    {
        Debug.Log("Im slashing");
        animator.SetBool("isSlashing", true);
        isAttacking = true;
    }
    public void EnableSlashHitbox()
    {
        attackHitboxes[0].SetActive(true);
    }
    public void DIsableSlashHitbox()
    {
        attackHitboxes[0].SetActive(false);
    }
    #endregion

    #region Whirlwind
    public void Whirlwind()
    {
        Debug.Log("Im whirlwinding");
        animator.SetBool("isWhirlwinding", true);
        age.speed = spinSpeed;
        isMoving = true;
        isAttacking = true;
    }
    public void EnableWhirlwindHitbox()
    {
        attackHitboxes[1].SetActive(true);
    }
    public void DssableWhirlwindHitbox()
    {
        attackHitboxes[1].SetActive(false);
    }
    #endregion

    #region OverHeadSwing
    public void OverHeadSwing()
    {
        Debug.Log("Im swinging");
        animator.SetBool("isOvrSwinging", true);
        isAttacking = true;
    }
    public void EnableOverHeadSwingHitbox()
    {
        attackHitboxes[2].SetActive(true);
    }
    public void DisableOverHeadSwingHitbox()
    {
        attackHitboxes[2].SetActive(false);
    }
    #endregion

    public void Explode()
    {
        Debug.Log("Im exploding");
        animator.SetBool("imExploding", true);
        hasExploded = true;
        isAttacking = true;
    }
    public void Charge()
    {
        Debug.Log("Im charging");
        animator.SetBool("isCharging", true);
        attackCounter += 1;
        isAttacking = true;
    }
    public void SlamAttack()
    {
        Debug.Log("Im slamming");
        animator.SetBool("isSlamming", true);
        isAttacking = true;
    }
    public void LazerBeam()
    {
        Debug.Log("Im firin mah lazer");
        animator.SetBool("isIdling2", false);
        animator.SetBool("isFiringLazer", true);
        isAttacking = true;
    }
    public void Death()
    {
        Debug.Log("Im Dying");
        animator.SetBool("isIdling2", false);
        animator.SetBool("isCharging", false);
        animator.SetBool("isSlamming", false);
        animator.SetBool("isRunning2", false);
        animator.SetBool("isFiringLazer", false);
        animator.SetBool("isPunching", false);
        animator.SetBool("isDying", true);
        isAttacking = true;
    }
    public void Idle()
    {
        attackCounter += 1;
        isAttacking = false;
        isMoving = false;
        animator.SetBool("isIdling", false);
        animator.SetBool("isTaunting", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isSlashing", false);
        animator.SetBool("isWhirlwinding", false);
        animator.SetBool("isOvrSwinging", false);
    }
    public void Idles()
    {
        animator.SetBool("isIdling", true);
        isAttacking = true;
    }
    public void EndChain()
    {
        attackCounter = 0;
        isAttacking = false;
        animator.SetBool("isIdling", false);
        animator.SetBool("isTaunting", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isSlashing", false);
        animator.SetBool("isWhirlwinding", false);
        animator.SetBool("isOvrSwinging", false);
    }
    public void endIdle()
    {
        animator.SetBool("isIdling", false);
        isAttacking = false;
    }
    public void Idle2()
    {
        animator.SetBool("isIdling2", true);
    }
    public void endIdle2()
    {
        isAttacking = false;
    }
}

