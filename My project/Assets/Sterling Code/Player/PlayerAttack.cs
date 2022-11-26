using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerAttack : MonoBehaviour
{

    PlayerMovement playerMovement;
    PlayerInput playerInput;


    public Collider attackArea;
    

    public Transform attackPosition;
    public Transform enmeyPosition;

    public LayerMask whatIsHittable;

    private float timer;
    public float startTimerAtThisValue;
    public float timeScalar;

    private bool isAttacking = false;


    private float delay = .3f;

    public float knockbackTimer;
    public float knockbackStrength;
    public int damage = 10;
    private Vector3 direction;
    private Rigidbody obj;

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
        {
            Attack();
        }

        if(obj != null)
        {
            direction = (obj.transform.position - attackPosition.position).normalized;

        }
    }

    private void Attack()
    {
        if (isAttacking)
            return;
        attackArea.enabled = true;
        OnTriggerEnter(attackArea);
        isAttacking = true;
        StartCoroutine(DelayAttack());                                                                              

    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;

    }

    private void OnTriggerEnter(Collider attackArea)
    {
       
        if(whatIsHittable == (whatIsHittable | (1 << attackArea.transform.gameObject.layer)))
        {
            obj = attackArea.gameObject.GetComponent<Rigidbody>();
            if (obj != null)
            {
                HitSomething(direction, obj);
            }
            else
                return;
        }
    }

    //Temporary
    private void HitSomething(Vector3 direction, Rigidbody obj)
    {
        //timer here 
        //if tag is enemy
        Invoke("AddKnockback", knockbackTimer);

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
        print("KnockBack");
    }
/*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPosition.position, attackRadius);

        Vector3 direction = enmeyPosition.position -attackPosition.position;
        Gizmos.DrawLine(attackPosition.position, direction.normalized + attackPosition.position);
        
    }*/

}
