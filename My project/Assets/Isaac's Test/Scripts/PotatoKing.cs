using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoKing : MonoBehaviour
{
    private Animator animator;
    private GameObject player;
    [Header("Basic attributes")]
    public int attackCounter = 0;
    public bool isAttacking = false;
    public Enemy enemyScript;
    public bool isStunned;
    public bool isIdle;
    public float groundYLevel;
    public float stunDuration;
    [Header("Roots Attack")]
    public GameObject roots;
    public float rootsAttackSpeed;
    private float rootsAttackspd;
    public float rootsLifetime;
    public float rootsYLevel;
    [Header("Projectile attributes")]
    public GameObject projectilePrefab;
    public Transform spawner;
    public float projectileSpeed;
    public float projectileLifetime;
    [Header("Charge Attack")]
    public float chargeSpeed;
    public float chargeWaitBeforeCharging;
    public GameObject burrowMoveEffect;
    public GameObject unborrowEffect; 
    public float chargeYLevel;
    [Header("Burrow")]
    public List<Transform> unborrowLocations;
    public float timeToUnborrow;
    public GameObject EnemyDirtSpawner;
    public float EnemyDirtSpawnSpeed;
    public int enemyMax;
    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(rootsAttackspd <= 0)
        {
            Roots();
        }
        if (!isAttacking && !isStunned && !isIdle)
        {
            switch (attackCounter)
            {
                case (0):
                    ShootPea();
                    break;
                case (1):
                    Charge();
                    break;
                case (2):
                    Burrow();
                    break;
                case (3):
                    Idle();
                    break;
                default:
                    break;
            }
        }
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void ShootPea()
    {
        Debug.Log("Im spitting");
        animator.SetBool("isSpitting", true);
        isAttacking = true;
        this.gameObject.transform.LookAt(player.transform);
    }
    public void SummonPea()
    {
        GameObject projectile = Instantiate(projectilePrefab, spawner.position, spawner.transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = spawner.forward * projectileSpeed;
        Destroy(projectile,projectileLifetime);
    }
    public void Roots()
    {
        Vector3 spawnLocation = new Vector3(player.transform.position.x, rootsYLevel, player.transform.position.z);
        GameObject inGameAttack1 = Instantiate(roots, spawnLocation, player.transform.rotation);
        Destroy(inGameAttack1, rootsLifetime);
        rootsAttackspd = rootsAttackSpeed;
    }
    public void Charge()
    {
        animator.SetBool("imBurrowing", true);
        //burrowMoveEffect.SetActive(true);
        isAttacking = true;
        StartCoroutine("ChargeMotion",(chargeWaitBeforeCharging));
    }
    IEnumerator ChargeMotion(float secs)
    {
        yield return new WaitForSeconds(secs);
        Vector3 originalLocation = new Vector3(this.transform.position.x, chargeYLevel, this.transform.position.z);
        Vector3 burrowLocation = new Vector3(player.transform.position.x, chargeYLevel, player.transform.position.z);
        this.transform.position = Vector3.Lerp(originalLocation, burrowLocation, chargeSpeed);
    }
    public void Burrow()
    {
        Debug.Log("Im Burrowing");
        animator.SetBool("imBurrowing", true);
        //burrowMoveEffect.SetActive(true);
        int unborrowLocation = Random.Range(0,unborrowLocations.Count);
        this.transform.position = Vector3.Lerp(this.transform.position, unborrowLocations[unborrowLocation].position, chargeSpeed);
        StartCoroutine("UnBorrow", timeToUnborrow);
        isAttacking = true;
    }
    IEnumerator UnBorrow(float secs)
    {
        yield return new WaitForSeconds(secs);
        animator.SetBool("imUnBurrowing", true);
    }
    public void SummonEnemy()
    {
        if(enemies.Length < enemyMax)
        {
            GameObject Seed = Instantiate(EnemyDirtSpawner, spawner.position, transform.rotation);
            Rigidbody rb = Seed.GetComponent<Rigidbody>();
            float angle = Random.Range(0f, 360f); // choose a random angle in degrees
            Vector3 axis = Random.insideUnitSphere; // choose a random axis
            Quaternion rotation = Quaternion.AngleAxis(angle, axis); // create a rotation around the random axis
            Vector3 upWithVariance = rotation * Vector3.up; // apply the rotation to the Vector3.up vector
            rb.AddForce(upWithVariance * EnemyDirtSpawnSpeed, ForceMode.Impulse);
        }
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
        isAttacking = true;
    }
    public void endAnimation()
    {
        isAttacking = false;
        attackCounter += 1;
        animator.SetBool("isDying", false);
        animator.SetBool("imUnBurrowing", false);
        animator.SetBool("imBurrowing", false);
        animator.SetBool("isIdling", false);
        animator.SetBool("isSpitting", false);
    }
    public void endChain()
    {
        isAttacking = false;
        attackCounter = 0;
        animator.SetBool("isDying", false);
        animator.SetBool("imUnBurrowing", false);
        animator.SetBool("imBurrowing", false);
        animator.SetBool("isSpitting", false);
        animator.SetBool("isIdling", false);
    }
    public void DebugAnimation()
    {
        animator.SetBool("isDying", false);
        animator.SetBool("imUnBurrowing", false);
        animator.SetBool("imBurrowing", false);
        animator.SetBool("isSpitting", false);
    }
}
