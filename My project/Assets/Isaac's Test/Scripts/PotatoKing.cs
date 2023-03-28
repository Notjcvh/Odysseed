using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoKing : MonoBehaviour
{
    private Rigidbody rb;
    [Header("Attack attributes")]
    public int attackCounter = 0;
    public bool isAttacking = false;
    public int healthThreshold = 50;
    public Enemy enemyScript;
    public bool isStunned;
    public bool isBuried;
    [Header("Phase 2")]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking && !isStunned)
        {
            switch (attackCounter)
            {
                case (0):
                    ShootPea();
                    break;
                case (1):
                    Whirlwind();
                    break;
                case (2):
                    Charge();
                    break;
                case (3):
                    WaterSlicer();
                    break;
                default:
                    break;
            }
        }

        if (isStunned)
        {
            attackCounter = 0;
        }
    }

    public void ShootPea()
    {
        Debug.Log("Im taunting");
        animator.SetBool("isTaunting", true);
        attackCounter += 1;
        isAttacking = true;
    }
    public void Whirlwind()
    {
        Debug.Log("Im whirlwinding");
        animator.SetBool("isWhirlwinding", true);
        attackCounter += 1;
        isAttacking = true;
    }
    public void Charge()
    {
        Debug.Log("Im charging");
        animator.SetBool("isCharging", true);
        attackCounter += 1;
        isAttacking = true;
    }
    public void WaterSlicer()
    {
        Debug.Log("Im slicing");
        animator.SetBool("isSlicing", true);
        attackCounter = 0;
        isAttacking = true;
    }
    public void Explode()
    {
        Debug.Log("Im exploding");
        animator.SetBool("imExploding", true);
        hasExploded = true;
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
        animator.SetBool("isFiringLazer", true);
        isAttacking = true;
    }
    public void Death()
    {
        Debug.Log("Im Dying");
        animator.SetBool("isDying", true);
        isAttacking = true;
    }
    public void Idle()
    {
        animator.SetBool("isIdling", true);
    }
    public void endIdle()
    {
        isAttacking = false;
    }

}
