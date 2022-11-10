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

    public float knockbackTimer;
    public float knockbackStrength;
    public int damage = 1;
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
        if (isAttacking == false)
        {           
            if (playerInput.attack)
            {
                Debug.Log("Player Attacked");
                attackArea.enabled = true;
                OnTriggerEnter(attackArea);
                isAttacking = true;
                playerMovement.stopMovementEvent = true;
            }
        }
        if (isAttacking)
        {
            timer = startTimerAtThisValue;
            timer -= Time.deltaTime * timeScalar;

            if (timer <= 0)
            {
                //Destroy Instance of the prefab
                timer = 0;
                isAttacking = false;
                attackArea.enabled = false;
                playerMovement.stopMovementEvent = false;
            }
        }
        if(obj != null)
        {
            direction = (obj.transform.position - attackPosition.position).normalized;

        }
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
            obj.SendMessage("DisableAI");
            obj.SendMessage("TakeDamage", damage);
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
