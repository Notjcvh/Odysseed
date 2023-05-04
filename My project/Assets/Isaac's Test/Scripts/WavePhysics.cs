using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WavePhysics : MonoBehaviour
{
    public Rigidbody rb;
    public Transform player;
    public float lifetime;
    public int damage;
    public float strength;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, lifetime);
        rb.velocity = player.forward * 20;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HitSomething(collision);
    }

    private void HitSomething(Collision obj)
    {
        switch (obj.gameObject.tag)
        {
            case ("Enemy"):
                obj.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                obj.gameObject.GetComponent<Enemy>().isStunned = true;
                AddKnockback(obj.gameObject.GetComponent<Rigidbody>());
                break;
            case ("Boss"):
                obj.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                break;
            default:
                break;
        }
       
    }
    private void AddKnockback(Rigidbody body)
    {
        //Dot product
        Vector3 targetDirection = (body.position - this.transform.position).normalized;
        // float dotProduct = Vector3.Dot(targetDirection, origin.position);
        
        targetDirection.y = 0;
        body.AddForce(targetDirection * strength, ForceMode.Impulse);
        
    }


}
