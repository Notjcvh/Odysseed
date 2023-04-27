using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HitCollider : MonoBehaviour
{
    public Transform origin;
    public LayerMask whatIsHittable;
    public PhysicsBehaviours behaviours;
    public int damage;
    public float timer;
    public float strength;

    public PlayerAttack playerAttack;  // use this member to determine the origin from target to enemy 

    public List<Rigidbody> hittableObjects;
    public TargetGroup targetGroup;

     
    private void Awake()
    {
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        if(playerAttack != null )
        {
            playerAttack.colliders.Add(gameObject.name, this);
        }
        targetGroup = GameObject.FindGameObjectWithTag("TargetGroup").GetComponent<TargetGroup>();
    }

    public virtual void MyBehaviour(PlayerAttack.PlayerCollider collider)
    {
        origin = this.transform;
       // whatIsHittable = collider.whatIsHittable;
        behaviours = collider.behaviours;
        damage = collider.damage;
        timer = collider.timer;
        strength = collider.strength;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (whatIsHittable == (whatIsHittable | (1 << other.transform.gameObject.layer))) // Bitwise equation: layermask == (layermask | 1 << layermask)
        {
            if(!hittableObjects.Contains(other.attachedRigidbody))
            {
                hittableObjects.Add(other.gameObject.GetComponent<Rigidbody>());
            }
        }

        for (int i = 0; i < hittableObjects.Count; i++)
        {
           // Debug.Log("Object " + (i + 1) + ": " + hittableObjects[i].gameObject.name);
            HitSomething(hittableObjects[i]);
        }
    }

    public virtual void HitSomething(Rigidbody obj)
    {
       // Debug.Log("Object " + obj.gameObject.name + "belongs to group " + obj.tag);
        switch (obj.tag)
        {
            case ("Enemy"):
                obj.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                break;
            case ("Boss"):
                obj.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                break;
            default:
                break;
        }
        switch (behaviours)
        {
            case PhysicsBehaviours.None: //
                obj.GetComponent<Enemy>().AppliedForce(PhysicsBehaviours.None);
                AddKnockback(obj);
                break;
            case PhysicsBehaviours.AggresiveKnockback: // Last hit Combos 
                obj.GetComponent<Enemy>().AppliedForce(PhysicsBehaviours.AggresiveKnockback);
                AddKnockback(obj);
                break;
            case PhysicsBehaviours.KnockUp:
                AddKnockUp(obj);
                break;
            case PhysicsBehaviours.ContinousKnockback:
                break;
            case PhysicsBehaviours.Knockdown:
                AddKnockdown(obj);
                break;
            default:
                break;
        }
        ClearList();
    }


    public void ClearList()
    {
        hittableObjects.Clear();
    }

    public void ActivateAdditionalBehaviours()
    {
        Debug.Log("Activated");
        //doing other things
       
    }

    public virtual void AddKnockback(Rigidbody body)
    {
       


        //Dot product
            Vector3 targetDirection = (body.position - origin.position).normalized;
           // float dotProduct = Vector3.Dot(targetDirection, origin.position);
             targetDirection.y = 0;
             body.AddForce(targetDirection * strength, ForceMode.Impulse);
    }
    private void AddKnockUp(Rigidbody body)
    {   
        body.AddForce(Vector3.up * strength, ForceMode.Impulse);
       // targetGroup.Add(body.transform);
       // targetGroup.timer = timer;
    }
    private void AddKnockdown(Rigidbody body)
    {
        body.AddForce(Vector3.down * strength, ForceMode.Impulse);
        targetGroup.cinemachineTargets.RemoveMember(body.transform);
    }
}