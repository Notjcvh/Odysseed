using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : HitCollider
{
    private GameObject player;
    private PlayerManger playerManger;
    private PlayerUI playerUI;


    [Range(0, 150)] private float staminaBar = 100; // the stamina bar maximum is 100
   // private int damage = 10; 
    public float regenAmount;
    public float delay;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManger = player.GetComponent<PlayerManger>();
        playerUI = player.GetComponent<PlayerUI>();
    }

    public override void MyBehaviour(PlayerAttack.PlayerCollider collider)
    {
        origin = this.transform.parent; // the player
        // whatIsHittable = collider.whatIsHittable;
        behaviours = collider.behaviours;
        damage = collider.damage; // each time the enemy enters the trigger the shield takes damage
        timer = collider.timer;
        strength = collider.strength;
    }

    private void Update()
    {
       
    }


    public void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Object " + obj.gameObject.name + "belongs to group " + obj.tag);
        switch (other.tag)
        {
            case ("Enemy"):
                
                break;
        }
    }

    public override void HitSomething(Rigidbody obj)
    {
        switch (behaviours)
        {
            case PhysicsBehaviours.AggresiveKnockback:
                AddKnockback(obj);
                playerManger.Blocked(damage);
                break;
            default:
                break;
        }
        ClearList();
    }

    void call()
    {
        playerManger.Blocked(damage);
    }

    public override void AddKnockback(Rigidbody body)
    {
        //This should only be applied once


        Debug.Log("Blocked add knockback to: " + body.name);
        body.GetComponent<Enemy>().TakeDamage(0);


        //Dot product
        Vector3 targetDirection = (body.position - origin.position).normalized;
        float dotProduct = Vector3.Dot(targetDirection, origin.position);

        targetDirection.y = 0;
        body.AddForce(targetDirection * strength, ForceMode.Impulse);

      
    }








}
