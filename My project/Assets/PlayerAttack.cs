using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerMovement playerMovement; 

    private float timer;
    public float startTimerAtThisValue;
    public float timeScalar;


    public Transform attackPosition;

    public LayerMask whatIsHittable;
    public float attackRadius;
    private bool isAttacking = false;

    private void Start()
    {
        playerMovement = playerMovement.GetComponent<PlayerMovement>();
    }



    // Update is called once per frame
    void Update()
    {
        
        if(isAttacking == false)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Debug.Log("Attack Button Pressed");
                isAttacking = true;
                Collider[] enemiestoDamage = Physics.OverlapSphere(attackPosition.position, attackRadius, whatIsHittable);
               // print("Stop Movemnt");
                playerMovement.stopMovement = true;

            }

        }

        if(isAttacking)
        {
            timer = startTimerAtThisValue;
            timer -=  Time.deltaTime * timeScalar;

            if(timer <= 0)
            {
                

                timer = 0;
                isAttacking = false;
                //Debug.Log("Can Move Now");
                playerMovement.stopMovement = false;
            }

        }




    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPosition.position, attackRadius);
        
    }

}
