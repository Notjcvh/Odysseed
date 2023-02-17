using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Referencing")]
    public Collider attackArea;

    // second collider to launch heavy 
    
    public Transform playerPos;
    public Transform attackPosition;
    public Transform enmeyPosition;
    public LayerMask whatIsHittable;

    public Animator animator;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput; 
    private Rigidbody obj;
    private Vector3 direction;

    [Header("Attack")]
    private int attackType;
    public float delayAttack = 1f;
    public int damage = 10;
    private bool isAttacking;

    private bool isLunging = false;
    public Transform lerpPosition;
  //  public Transform orgLerpPos;
    public float lerpduration;
    public LayerMask playerCollionMask;
    IEnumerator co;

    public float cooldownTime = 2f;
    private float nextFireTime = 0f;

    [Header("Knockback")]
    public float knockbackTimer;
    public float knockbackStrength;

    [Header("Combo")]
    public int attackNumber;

    #region Unity Functions
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        playerInput = GetComponent<PlayerInput>();
        
        attackArea.enabled = false;       
    }
    // Update is called once per frame
    void Update()
    {
       
        if(obj != null)
          direction = (obj.transform.position - attackPosition.position).normalized; // finding the direction from attackPos to Obj rigidbody. In update so Knockback happen for the full time between frames

        if (playerInput.attack)
        {
            Attack(0);
        }

        if (playerInput.secondaryAttack)
        {
            Attack(1);
        }
    }
    #endregion

    #region Private Functions
    private void Attack(int attackInt)
    {
        isAttacking = true;

        playerMovement.stopMovementEvent = true;
        attackArea.enabled = true;
        animator.SetBool("Active", true);
      

        animator.SetInteger("Attack Type", attackInt);

        //checking for attack Type and is Grounded for Knockback
        bool isAnimationActive = animator.GetBool("Active");
        switch (attackInt, isAnimationActive, playerMovement.IsGrounded())
        {
            //ground attack checks 
            case (0, true, true):
                Debug.Log("Light Attack");
                break;
            case (1, true, true):
                Debug.Log("Heavy Attack");
                break;
            // air attack checks 
            case (0, true, false):
                Debug.Log("Light Attack in Air");
                break;
            case (1, true, false):
                Debug.Log("Heavy Attack in Air");
                break;
            default:
                Debug.Log("null");
                break;
        }
        OnTriggerEnter(attackArea);
        StartCoroutine(DelayAttack());
    }


    // hos does this affect attacking 
    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delayAttack);
    }


    private void OnTriggerEnter(Collider attackArea) // if an object has collided with the attacksphere while it is active 
    {
        // if the 3rd hit 
        if(whatIsHittable == (whatIsHittable | (1 << attackArea.transform.gameObject.layer))) // Bitwise equation: layermask == (layermask | 1 << layermask)
        {
            obj = attackArea.gameObject.GetComponent<Rigidbody>();
            if (obj != null)
              HitSomething(direction, obj);
            else
                return;
        }
    }

    // Create Stun Stop enemey
    private void HitSomething(Vector3 direction, Rigidbody obj)
    {

        //int attackTypeConnected = attackType; // might be a problem with multiple clicks 
       
        
        //if tag is enemy
        if (obj.tag == "Enemy")
        {
            DamagePopUp.Create(obj.transform.position, damage);
            obj.SendMessage("DisableAI");
            obj.gameObject.GetComponent<Enemy>().ModifiyHealth(damage / 10);
            obj.gameObject.GetComponent<EnemyStats>().VisualizeDamage(obj);
            obj.SendMessage("TakeDamage", damage / 10);

        }
        else if (obj.tag == "SpecialEnemy")
        {
            DamagePopUp.Create(obj.transform.position, damage);
            obj.SendMessage("DisableAI");
            obj.gameObject.GetComponent<SpecialEnemy>().ModifiyHealth(damage / 10);
            obj.gameObject.GetComponent<EnemyStats>().VisualizeDamage(obj);
            obj.SendMessage("TakeDamage", damage / 10);
        }
        if (obj.tag == "Boss")
        {
            obj.SendMessage("TakeDamage", damage / 10);
        }
    }

    private void AddKnockback()
    {
        direction.y = 0;
        obj.AddForce(direction * knockbackStrength, ForceMode.Impulse);
    }
    #endregion


    #region Sliding Forward When Attacking 
    public void SlideForward()
    {
        co = MoveForwardWhenAttacking(transform.position, lerpPosition.position, lerpduration);
        StartCoroutine(co);
    }
    private IEnumerator MoveForwardWhenAttacking(Vector3 currentPostion, Vector3 endPosition, float time)
    {
        RaycastHit hit;
        float range = 2f;
        Ray ray = new Ray(currentPostion, transform.TransformDirection(Vector3.forward * range));

        Vector3 midpoint = Vector3.Lerp(currentPostion, endPosition, .5f);
        //this is for sliding 
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            if (Physics.Raycast(ray, range, playerCollionMask, QueryTriggerInteraction.Ignore))
            {
                transform.position = currentPostion;
            }
            else
                transform.position = Vector3.Lerp(currentPostion, midpoint, t);
            yield return null;
        }
    }
    #endregion



    public void isAnimationFinished()
    {
        isAttacking = false;
        playerMovement.stopMovementEvent = false;
        attackArea.enabled = false;
        animator.SetBool("Active", false);

    }
}
