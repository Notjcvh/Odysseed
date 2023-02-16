using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Referencing")]
    public Collider attackArea;

    // second collider to launch heavy 

    
    public Transform playerPos;
    public Transform attackPosition;
    public Transform enmeyPosition;
    public LayerMask whatIsHittable;

    public Animator animator;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput; 
    private Rigidbody obj;
    private Vector3 direction;
  
    [Header("Attack")]
    public float delayAttack = 1f;
    public int damage = 10;
    private bool isAttacking;


    


    public float cooldownTime = 2f;
    private float nextFireTime = 0f;

    [Header("Knockback")]
    public float knockbackTimer;
    public float knockbackStrength;

    [Header("Combo")]
    public int attackNumber;





    #region Unity Functions
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        playerInput = GetComponent<PlayerInput>();
        
        attackArea.enabled = false;       
    }
    // Update is called once per frame
    void Update()
    {
       
        if(obj != null)
          direction = (obj.transform.position - attackPosition.position).normalized; // finding the direction from attackPos to Obj rigidbody. In update so Knockback happen for the full time between frames

        if (playerInput.attack)
        {
            Attack(0);
        }

        if(playerInput.secondaryAttack)
        {
            Attack(1);
        }
    }
    #endregion

    #region Private FUnctions
    private void Attack(int attackType)
    {
        isAttacking = true;
        playerMovement.stopMovementEvent = true;
        attackArea.enabled = true;
        animator.SetBool("Active", true);
        animator.SetInteger("Attack Type", attackType);
        OnTriggerEnter(attackArea);
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delayAttack);
      
        attackNumber += 1;
    }


    // pass in the attack int 
    private void OnTriggerEnter(Collider attackArea) // if an object has collided with the attacksphere while it is active 
    {
        // if the 3rd hit 
        if(whatIsHittable == (whatIsHittable | (1 << attackArea.transform.gameObject.layer))) // Bitwise equation: layermask == (layermask | 1 << layermask)
        {

            Debug.Log(Convert.ToString(whatIsHittable, 2).PadLeft(32, '0'));
            obj = attackArea.gameObject.GetComponent<Rigidbody>();
            if (obj != null)
              HitSomething(direction, obj);
            else
                return;
        }
    }

    // Create Stun Stop enemey
    private void HitSomething(Vector3 direction, Rigidbody obj)
    {
        //timer here 
        //if tag is enemy
        if (obj.tag == "Enemy")
        {
            DamagePopUp.Create(obj.transform.position, damage);
            obj.SendMessage("DisableAI");
            obj.gameObject.GetComponent<Enemy>().ModifiyHealth(damage / 10);
            obj.gameObject.GetComponent<EnemyStats>().VisualizeDamage(obj);
            obj.SendMessage("TakeDamage", damage / 10);

        }
        else if (obj.tag == "SpecialEnemy")
        {
            DamagePopUp.Create(obj.transform.position, damage);
            obj.SendMessage("DisableAI");
            obj.gameObject.GetComponent<SpecialEnemy>().ModifiyHealth(damage / 10);
            obj.gameObject.GetComponent<EnemyStats>().VisualizeDamage(obj);
            obj.SendMessage("TakeDamage", damage / 10);
        }
        if (obj.tag == "Boss")
        {
            obj.SendMessage("TakeDamage", damage / 10);
        }

    }

    private void AddKnockback()
    {
        direction.y = 0;
        obj.AddForce(direction * knockbackStrength, ForceMode.Impulse);
    }
    #endregion



    public void isAnimationFinished()
    {
        isAttacking = false;
        playerMovement.stopMovementEvent = false;
        attackArea.enabled = false;
        animator.SetBool("Active", false);

    }
}
