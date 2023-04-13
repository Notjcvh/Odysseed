using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoKing : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private GameObject player;
    [Header("Basic attributes")]
    public int attackCounter = 0;
    public bool isAttacking = false;
    public int healthThreshold = 50;
    public Enemy enemyScript;
    public bool isStunned;
    public bool isIdle;
    public float groundYLevel;
    public float stunDuration;
    [Header("Roots Attack")]
    public GameObject roots;
    public float rootsLifetime;
    public float rootsYLevel;
    [Header("Projectile attributes")]
    public GameObject projectilePrefab;
    public Transform spawner;
    public float projectileSpeed;
    [Header("Charge Attack")]
    public float chargeSpeed;
    public float chargeWaitBeforeCharging;
    public GameObject burrowMoveEffect;
    public GameObject unborrowEffect; 
    public float chargeYLevel;
    [Header("Burrow")]
    public List<Transform> unborrowLocations;
    public float timeToUnborrow;

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
        if (!isAttacking && !isStunned && !isIdle)
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
                    Burrow();
                    break;
                default:
                    break;
            }
            if(isStunned)
            {
                IsStunned(stunDuration);
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
        Vector3 spawnLocation = new Vector3(player.transform.position.x, rootsYLevel, player.transform.position.z);
        GameObject inGameAttack1 = Instantiate(roots, spawnLocation, player.transform.rotation);
        Destroy(inGameAttack1, rootsLifetime);
    }
    public void Charge()
    {
        animator.SetBool("imBurrowing", true);
        attackCounter += 1;
        isAttacking = true;
        StartCoroutine("ChargeMotion",(chargeWaitBeforeCharging));
    }
    IEnumerator ChargeMotion(float secs)
    {
        yield return new WaitForSeconds(secs);
        Vector3 originalLocation = new Vector3(player.transform.position.x, chargeYLevel, player.transform.position.z);
        Vector3 burrowLocation = new Vector3(player.transform.position.x, chargeYLevel, player.transform.position.z);
        this.transform.position = Vector3.Lerp(originalLocation, burrowLocation, chargeSpeed);
    }
    public void Burrow()
    {
        Debug.Log("Im Burrowing");
        animator.SetBool("imBurrowing", true);
        attackCounter += 1;
        int unborrowLocation = Random.Range(0,unborrowLocations.Count);
        this.transform.position = unborrowLocations[unborrowLocation].position;
        StartCoroutine("UnBorrow", timeToUnborrow);
        isAttacking = true;
    }
    IEnumerator UnBorrow(float secs)
    {
        yield return new WaitForSeconds(secs);
        animator.SetBool("imBurrowing", true);
        isStunned = true;
    }

    public void IsStunned(float stunDuration)
    {
        animator.SetBool("isStunned", true);
        StartCoroutine("GetUnStunned", stunDuration);
    }
    IEnumerator GetUnStunned(float secs)
    {
        yield return new WaitForSeconds(secs);
        isStunned = true;
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
