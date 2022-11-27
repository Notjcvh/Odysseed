using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Referencing")]
    public Collider attackArea;
    public Transform attackPosition;
    public Transform enmeyPosition;
    public LayerMask whatIsHittable;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput; 
    private Rigidbody obj;
    private Vector3 direction;
  
    [Header("Attack")]
    public float delayAttack = .3f;
    public int damage = 10;
    private bool isAttacking;

    [Header("Knockback")]
    public float knockbackTimer;
    public float knockbackStrength;

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
        if(playerInput.attack)
          Attack();
        if(obj != null)
          direction = (obj.transform.position - attackPosition.position).normalized; // finding the direction from attackPos to Obj rigidbody. In update so Knockback happen for the full time between frames
    }
    #endregion

    #region Private FUnctions
    private void Attack()
    {
        if (isAttacking)
            return;
        attackArea.enabled = true;
        OnTriggerEnter(attackArea);
        isAttacking = true;
        playerMovement.stopMovementEvent = true;
        StartCoroutine(DelayAttack());                                                                              

    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delayAttack);
        isAttacking = false;
        playerMovement.stopMovementEvent = false;
        attackArea.enabled = false;
    }

    private void OnTriggerEnter(Collider attackArea) // if an object has collided with the attacksphere while it is active 
    {
        if(whatIsHittable == (whatIsHittable | (1 << attackArea.transform.gameObject.layer)))
        {
            obj = attackArea.gameObject.GetComponent<Rigidbody>();
            if (obj != null)
              HitSomething(direction, obj);
            else
                return;
        }
    }

    //Temporary
    private void HitSomething(Vector3 direction, Rigidbody obj)
    {
        //timer here 
        //if tag is enemy
        if (obj.tag == "Enemy")
        {
            Invoke("AddKnockback", knockbackTimer); // only applying knockback to what we consider an enemy
            DamagePopUp.Create(obj.transform.position, damage);
            obj.SendMessage("DisableAI");
            obj.gameObject.GetComponent<Enemy>().ModifiyHealth(damage / 10);
            obj.gameObject.GetComponent<EnemyStats>().VisualizeDamage(obj);
            obj.SendMessage("TakeDamage", damage / 10);

        }
        else if (obj.tag == "SpecialEnemy")
        {
            Invoke("AddKnockback", knockbackTimer);
            Debug.Log("Special hit");
            DamagePopUp.Create(obj.transform.position, damage);
            obj.SendMessage("DisableAI");
            obj.gameObject.GetComponent<SpecialEnemy>().ModifiyHealth(damage / 10);
            obj.gameObject.GetComponent<EnemyStats>().VisualizeDamage(obj);
            obj.SendMessage("TakeDamage", damage / 10);
        }
    }

    private void AddKnockback()
    {
        direction.y = 0;
        obj.AddForce(direction * knockbackStrength, ForceMode.Impulse);
    }
    #endregion
}
