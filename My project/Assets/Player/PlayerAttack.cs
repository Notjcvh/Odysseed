using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;



[RequireComponent(typeof(PlayerInput))]
public class PlayerAttack : MonoBehaviour 
{
    //Handle Inputs, Button Clicks, Collisions, and Possible Physics Applying Knockback 


    [Header("Referencing")]
    public Collider attackArea;
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
    public int inputType;
    public float delayAttack = 1f;
    public int damage = 10;
  

    public Transform lerpPosition;
    public float lerpduration;
    public LayerMask playerCollionMask;
    IEnumerator co;

    public float cooldownTime = 2f;
    private float nextFireTime = 0f;

    [Header("Knockback")]
    public float knockbackTimer;
    public float knockbackStrength;


    [Header("In Combo")]   
    public bool isAnimationActive;

    [Range(0, 3)]public  int lightAttackCounter;
    [Range(0, 3)]public int heavyAttackCounter;
    public float comboLifeCounter = 0;


    #region Unity Functions
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
    }   

    void Update()
    {
        if(obj != null)
          direction = (obj.transform.position - attackPosition.position).normalized; // finding the direction from attackPos to Obj rigidbody. In update so Knockback happen for the full time between frames
        if (playerInput.attack && isAnimationActive == false)
        {
            animator.SetBool("Attacking", true);
            inputType = 0;
            Attack(inputType);
            animator.SetInteger("Mouse Input", inputType);
        }
        else if (playerInput.secondaryAttack && isAnimationActive == false)
        {
            animator.SetBool("Attacking", true);
            inputType = 1;
            Attack(inputType);
            animator.SetInteger("Mouse Input", inputType);
        }

        if(comboLifeCounter > 0)
        {
            //begin countdown 
            comboLifeCounter -= 1 * Time.deltaTime;
            animator.SetFloat("ComboLifetime", comboLifeCounter);
  
        }
        else if(comboLifeCounter < 0)
        {
            comboLifeCounter = 0;
            animator.SetFloat("ComboLifetime", comboLifeCounter);
        }
      
    }
    #endregion

    #region Attacks and Combos
    private void Attack(int inputType)
    {
        playerMovement.stopMovementEvent = true;

        //Light Attacks
        if (inputType == 0)
        {
            lightAttackCounter++;
            switch (lightAttackCounter, playerMovement.IsGrounded())
            {
                #region Ground Light Attacks
                case (1, true): // Starter
                    Debug.Log("Light Starter");
                    animator.SetFloat("Starter Type", 0);
                    animator.SetFloat("ComboLifetime", 5f);
                    PlayAttackAnimation(lightAttackCounter);
                    break;
                case (2, true):
                    Debug.Log("Light Attack 2");
                    animator.SetFloat("ComboLifetime", 5f);
                    PlayAttackAnimation(lightAttackCounter);
                    break;
                case (3, true):
                    Debug.Log("Light Attack 3");
                    animator.SetFloat("ComboLifetime", 5f);
                    PlayAttackAnimation(lightAttackCounter);

                    break;
                case (4, true): // finisher
                    Debug.Log("Light Attack 4");
                    break;
                #endregion
                #region Air Light Attacks 
                case (1, false):
                    Debug.Log("Air Light Starter");
                    break;

                #endregion
                default:
                    break;
            }
        }
        else
        {
            heavyAttackCounter++;
            switch (heavyAttackCounter, playerMovement.IsGrounded())
            {
                #region Ground Heavy Attacks
                case (1, true):
                    Debug.Log("Heavy Starter");
                    animator.SetFloat("Starter Type", 1);
                    animator.SetFloat("ComboLifetime", 5f);
                    PlayAttackAnimation(heavyAttackCounter);
                    break;
                case (2, true):
                    Debug.Log("Heavy Attack 2");
                    animator.SetFloat("ComboLifetime", 5f);
                    PlayAttackAnimation(heavyAttackCounter);
                    break;
                case (3, true):
                    Debug.Log("Heavy Attack 3");
                    animator.SetFloat("ComboLifetime", 5f);
                    PlayAttackAnimation(heavyAttackCounter);
                    break;
                #endregion
                default:
                    break;
            }
            OnTriggerEnter(attackArea);
            StartCoroutine(DelayAttack()); // For Later what does this do
        }
    }
   
    private void PlayAttackAnimation(int value)
    {
        isAnimationActive = true;

        int sum = value;
        
        if(inputType == 0) // light attack 
        {
            
            animator.SetInteger("Attack Type", sum);
            heavyAttackCounter++;
        }
        else // heavy attack 
        {
            animator.SetInteger("Attack Type", sum);
            lightAttackCounter++;
        }

    }

    public void isAnimationFinished()
    {
        playerMovement.stopMovementEvent = false;
        animator.SetBool("Attacking", false);
        isAnimationActive = false;

        // check the max number, true?, reset 
        if(lightAttackCounter == 3)
        {
            lightAttackCounter = 0;
            heavyAttackCounter = 0;
            comboLifeCounter = 0;
            animator.SetInteger("Attack Type", 0);    
        }
        if(heavyAttackCounter == 3)
        {
            heavyAttackCounter = 0;
            heavyAttackCounter = 0;
            comboLifeCounter = 0;
            animator.SetInteger("Attack Type", 0);
        }

        //get combo timer
        comboLifeCounter = animator.GetFloat("ComboLifetime");
      
        
    }

 
    // how does this affect attacking 
    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delayAttack);
    }

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


    #region Move somewhere else 
    private void OnTriggerEnter(Collider attackArea) // if an object has collided with the attacksphere while it is active 
    {
        // if the 3rd hit 
        if (whatIsHittable == (whatIsHittable | (1 << attackArea.transform.gameObject.layer))) // Bitwise equation: layermask == (layermask | 1 << layermask)
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
    private void AddKnockUp()
    {
        direction.x = 0;
        direction.z = 0;
        direction.y = 1;
        obj.AddForce(direction * knockbackStrength, ForceMode.Impulse);
    }
    #endregion
    #endregion
}
