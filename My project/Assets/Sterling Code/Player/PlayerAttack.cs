using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Collider attackArea;

    public Transform attackPosition;
    public Transform enmeyPosition;

    public LayerMask whatIsHittable;

    private float timer;
    public float startTimerAtThisValue;

    public float timeScalar;

    public float attackRadius;
    private bool isAttacking = false;

    int damage = 1;

    public float knockbackStrength;

    private void Start()
    {
        playerMovement = playerMovement.GetComponent<PlayerMovement>();
        attackArea.enabled = false;
        
        
    }
    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false)
        {           
            if (Input.GetKey(KeyCode.Mouse0))
            {
                attackArea.enabled = true;
                OnTriggerEnter(attackArea);
                isAttacking = true;
                playerMovement.stopMovement = true;
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
                // Debug.Log("Step three");
                //Debug.Log("Can Move Now");
                playerMovement.stopMovement = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider attackArea)
    {       
        
        if(whatIsHittable == (whatIsHittable| (1 << attackArea.transform.gameObject.layer)))
        {
            Rigidbody obj = attackArea.gameObject.GetComponent<Rigidbody>();
            if (obj != null)
            {
                Vector3 direction = (obj.transform.position - attackPosition.position).normalized;
                HitSomething(direction, obj);
            }
        }
    }

    //Temporary
    private void HitSomething(Vector3 direction, Rigidbody obj)
    {
        //timer here 
        //if tag is enemy
        direction.y = 0;
        obj.AddForce(direction * knockbackStrength, ForceMode.Impulse);
        print("KnockBack");

        if (obj.tag == "Enemy")
            obj.SendMessage("TakeDamage", damage);
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
