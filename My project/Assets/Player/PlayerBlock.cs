using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : HitCollider
{

    private PlayerManger playerManger;
    private PlayerUI playerUI;
    public GameObject shield;
    [Range(0, 150)] public float staminaBar = 100; // the stamina bar maximum is 100
    public float regenAmount;
    public float delay;



    private void Start()
    {
        playerManger = GetComponent<PlayerManger>();
        playerUI = GetComponent<PlayerUI>();
    }


    private void Update()
    {

        

        if(playerManger.subStates != SubStates.Guarding)
        {
            shield.SetActive(false);

            if(playerManger.PlayerBlockHealth < 100 && playerManger.blocking != true)
            {
                playerManger.PlayerBlockHealth += regenAmount * Time.deltaTime;
                Debug.Log(playerManger.PlayerBlockHealth);
            }
        }
        else if(playerManger.subStates == SubStates.Guarding && playerManger.PlayerBlockHealth > 0)
        {
            shield.SetActive(true);
            MyBehaviour(new PlayerAttack.PlayerCollider(PhysicsBehaviours.AggresiveKnockback, 0, 5f, 40));
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
