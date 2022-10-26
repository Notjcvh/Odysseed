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
    public Transform enmeyPosition;

    public LayerMask whatIsHittable;
    public float attackRadius;
    private bool isAttacking = false;

    public float knockbackStrength;

    private void Start()
    {
        playerMovement = playerMovement.GetComponent<PlayerMovement>();
    }



    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Debug.Log("Step 1");
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
                Debug.Log("Step three");
                //Debug.Log("Can Move Now");
                playerMovement.stopMovement = false;
            }

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
       
            Collider[] objIsHit = Physics.OverlapSphere(attackPosition.position, attackRadius, whatIsHittable);
            for (int i = 0; i < objIsHit.Length; i++)
            {
                Rigidbody obj = objIsHit[i].GetComponent<Rigidbody>();

                if (obj != null)
                {
                    Debug.Log("Step two");
                    //collision point to obj
                    Vector3 direction = (attackPosition.position - collision.transform.position).normalized;
                    direction.y = 0;

                    obj.AddForce(direction * knockbackStrength, ForceMode.Impulse);
                    print("KnockBack");
                }
            }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPosition.position, attackRadius);

        Vector3 direction = enmeyPosition.position -attackPosition.position;
        Gizmos.DrawLine(attackPosition.position, direction.normalized + attackPosition.position);
        
    }

}
