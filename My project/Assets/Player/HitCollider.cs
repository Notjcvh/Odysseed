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

    public void MyBehaviour(PlayerAttack.PlayerCollider collider)
    {
        origin = collider.origin;
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
            //Check for all gameobjects 
            hittableObjects.Add(other.gameObject.GetComponent<Rigidbody>());
            HitSomething(hittableObjects);
        }
    }

    public void ClearList()
    {
        hittableObjects.Clear();
        Debug.Log("List Cleared");
    }

    private void HitSomething(List<Rigidbody> objs)
    {
        //Deal Damage 
        foreach (var item in objs)
        {
            switch (item.tag)
            {
                case("Enemy"):
                    //   DamagePopUp.Create(item.transform.position, damage);
                    item.SendMessage("DisableAI", 100);
                    //  item.gameObject.GetComponent<Enemy>().ModifiyHealth(damage / 10);
                    //   //obj.gameObject.GetComponent<EnemyStats>().VisualizeDamage(obj);
                    //  item.SendMessage("TakeDamage", damage / 10); 
                    break;
                case ("SpecialEnemy"):
                    //  DamagePopUp.Create(item.transform.position, damage);
                    item.SendMessage("DisableAI");
                    //   item.gameObject.GetComponent<SpecialEnemy>().ModifiyHealth(damage / 10);
                    // item.gameObject.GetComponent<EnemyStats>().VisualizeDamage(item);
                    //  item.SendMessage("TakeDamage", damage / 10);
                    break;
                case ("Boss"):
                    //  item.SendMessage("TakeDamage", damage / 10);
                    break;
                default:
                    break;
            }
        }
    }

     public void ActivateAdditionalBehaviours()
    {
        //doing other things
        foreach (var item in hittableObjects)
        {
            switch (behaviours)
            {
                case PhysicsBehaviours.Knockback:
                    AddKnockback(item);
                    break;
                case PhysicsBehaviours.KnockUp:
                    AddKnockUp(item);
                    break;
                case PhysicsBehaviours.ContinousKnockback:
                    break;
                default:
                    break;
            }
        }
    }

    private void AddKnockback(Rigidbody body)
    {
        //Dot product
     //   Vector3 targetDirection = (body.position - originTarget.position).normalized;
       // float dotProduct = Vector3.Dot(targetDirection, originTarget.position);

        //  direction.y = 0;
        // obj.AddForce(direction * knockbackStrength, ForceMode.Impulse);
    }
    private void AddKnockUp(Rigidbody body)
    {   
        body.AddForce(Vector3.up * strength, ForceMode.Impulse);
        targetGroup.Add(body.transform);
    }


}