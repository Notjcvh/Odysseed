using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoKing : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject player;
    [Header("Basic attributes")]
    public int attackCounter = 0;
    public bool isAttacking = false;
    public int healthThreshold = 50;
    public Enemy enemyScript;
    public bool isStunned;
    public bool isBuried;
    [Header("Roots Attack")]
    public GameObject roots;
    public float rootsLifetime;
    [Header("Projectile attributes")]
    public GameObject projectilePrefab;
    public Transform spawner;
    public float projectileSpeed;
    [Header("Charge Attack")]
    public float chargeSpeed;
    [Header("Phase 2")]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
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
                    Roots();
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
        Debug.Log("Im spitting");
        animator.SetBool("isSpitting", true);
        attackCounter += 1;
        isAttacking = true;
        this.gameObject.transform.LookAt(player.transform);
        GameObject projectile = Instantiate(projectilePrefab, spawner.position, spawner.transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = spawner.forward * projectileSpeed;
    }
    public void Roots()
    {
        Debug.Log("Im spawning roots");
        animator.SetBool("isWhirlwinding", true);
        attackCounter += 1;
        isAttacking = true;
        Vector3 spawnLocation = new Vector3(player.transform.position.x, 20, player.transform.position.z);
        GameObject inGameAttack1 = Instantiate(roots, spawnLocation, player.transform.rotation);
        Destroy(inGameAttack1, rootsLifetime);
    }
    public void Charge()
    {
        Debug.Log("Im charging");
        animator.SetBool("isCharging", true);
        attackCounter += 1;
        isAttacking = true;
        //public Vector3 target = new Vector3(PlayerAttack.)

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
