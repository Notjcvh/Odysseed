using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : HitCollider
{

    private PlayerManger playerManger;
    public GameObject shield;
    [Range(0, 150)] public float staminaBar = 100; // the stamina bar maximum is 100
    public float regenAmount;
    public float delay;



    private void Start()
    {
        playerManger = GetComponent<PlayerManger>();
    }


    private void Update()
    {
        staminaBar = playerManger.PlayerBlockHealth;

        if(staminaBar > 100)
        {
            staminaBar = 100;
        }

        if(playerManger.subStates == SubStates.Guarding)
        {
            shield.SetActive(true);
        }
        else
        {
            shield.SetActive(false);

            if(staminaBar < 100)
            {
                staminaBar += regenAmount * Time.deltaTime;
            }
        }
    }

    public override void MyBehaviour(PlayerAttack.PlayerCollider collider)
    {
        origin = this.transform;
        // whatIsHittable = collider.whatIsHittable;
        behaviours = collider.behaviours;
        damage = collider.damage;
        timer = collider.timer;
        strength = collider.strength;
    }

    public override void HitSomething(Rigidbody obj)
    {
        switch (behaviours)
        {
            case PhysicsBehaviours.AggresiveKnockback:
                AddKnockback(obj);
                break;
            default:
                break;
        }
        ClearList();
    }


    public override void AddKnockback(Rigidbody body)
    {

        Debug.Log("Blocked add knockback to: " + body.name);
        //Dot product
        //Vector3 targetDirection = (body.position - origin.position).normalized;
        // float dotProduct = Vector3.Dot(targetDirection, origin.position);

        //targetDirection.y = 0;
        //body.AddForce(targetDirection * strength, ForceMode.Impulse);
    }








}
