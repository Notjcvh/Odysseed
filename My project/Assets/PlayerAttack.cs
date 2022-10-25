using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBetweenAttacks;
    public float startTimeBetweenAttacks;


    public Transform attackPosition;

    public LayerMask whatIsHittable;
    public float attackRadius;

    // Update is called once per frame
    void Update()
    {

        if (timeBetweenAttacks <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Debug.Log("Attack Button Pressed");
                //  Collider[] enemiestoDamage = Physics.OverlapSphere(attackPosition.position, attackRange, whatIsHittable);
            }

            timeBetweenAttacks = startTimeBetweenAttacks;
        }
        else
            timeBetweenAttacks -= Time.deltaTime;


    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPosition.position, attackRadius);
    }

}
