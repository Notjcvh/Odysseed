using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using System.Reflection;
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

    [Header("Knockback or Launch Up")]
    public float knockbackTimer;
    public float knockbackStrength;
    public bool canKnockback;
    public bool canLaunchUp;

    [Header("In Combo")]   
    public bool isAnimationActive;
    // These have to be Integers to work with the Animator 
    [Range(0, 3)]public int lightAttackCounter; private float lightAttackMaxRange;
    [Range(0, 3)]public int heavyAttackCounter; private float heavyAttackMaxRange;   
    public float comboLifeCounter = 0;


    [Header("Animation Multiplier")]
   [Range(0,10)] public float animMultiplier;


    #region Unity Functions
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();

        var lightAttackRange = typeof(PlayerAttack).GetField(nameof(PlayerAttack.lightAttackCounter)).GetCustomAttribute<RangeAttribute>();
        lightAttackMaxRange = lightAttackRange.max;
        var heavyAttackRange = typeof(PlayerAttack).GetField(nameof(PlayerAttack.heavyAttackCounter)).GetCustomAttribute<RangeAttribute>();
        heavyAttackMaxRange = heavyAttackRange.max;
    }   

    void Update()
    {
        if(obj != null)
          direction = (obj.transform.position - attackPosition.position).normalized; // finding the direction from attackPos to Obj rigidbody. In update so Knockback happen for the full time between frames

     


        //Handing Mouse inputs
        if (playerInput.attack && isAnimationActive == false)
        {
            animator.SetBool("Attacking", true);
            inputType = 0;
            animator.SetInteger("Mouse Input", inputType);
            Attack(inputType);
         
        }
        else if (playerInput.secondaryAttack && isAnimationActive == false)
        {
            animator.SetBool("Attacking", true);
            inputType = 1;
            animator.SetInteger("Mouse Input", inputType);
            Attack(inputType);
            
        }

        //Handeling starting and Reseting Combo timer
        if(comboLifeCounter > 0)
        {
            //begin countdown 
            comboLifeCounter -= 1 * Time.deltaTime;
            // bool isDashing
            //bool isJumping

        }
        else if(comboLifeCounter < 0 || playerMovement.targetSpeed != 0)
        {
            // if player moves than we set to -1 to move to next transition
            comboLifeCounter = -1;
            animator.SetFloat("ComboLifetime", comboLifeCounter);
            ResetCombo();
            //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Drop Ground Combo") || animator.GetCurrentAnimatorStateInfo(0).IsName("Drop")) // Checking for if Combo is dropped 

        }
        
        
        if(playerMovement.targetSpeed != 0 && comboLifeCounter > 0)
        {
            comboLifeCounter = -1;
            print(comboLifeCounter);
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
                    canKnockback = true;
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
                    canKnockback = true;
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
                    canLaunchUp = true;
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
                    canLaunchUp = true;
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

        canKnockback = false;
        canLaunchUp = false;

        if(lightAttackCounter == lightAttackMaxRange|| heavyAttackCounter == heavyAttackMaxRange) // Finisher end of the Combo 
            ResetCombo();
        else
           comboLifeCounter = animator.GetFloat("ComboLifetime");
    }

    private void ResetCombo()
    {
        Debug.Log("ran");
        comboLifeCounter = 0;
        animator.SetFloat("ComboLifetime", comboLifeCounter);
        lightAttackCounter = 0;
        heavyAttackCounter = 0;
        animator.SetInteger("Attack Type", 0);
    }

 
    // how does this affect attacking 
    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delayAttack);
    }

    #region Hit Detection and Knockback 
    private void OnTriggerEnter(Collider attackArea) // if an object has collided with the attacksphere while it is active 
    {
        // if the 3rd hit 
        if (whatIsHittable == (whatIsHittable | (1 << attackArea.transform.gameObject.layer))) // Bitwise equation: layermask == (layermask | 1 << layermask)
        {
            obj = attackArea.gameObject.GetComponent<Rigidbody>();
            if (obj != null)
            {
                HitSomething(direction, obj);
            }
                
            else
                return;
        }
    }

    // Create Stun Stop enemey
    private void HitSomething(Vector3 direction, Rigidbody obj)
    {
        //check tag of the enemies
        // then check if this is the final hit in the combo, and the type of input pressed 
        // then create the response 
        if (obj.tag == "Enemy")
        {
            DamagePopUp.Create(obj.transform.position, damage);
            obj.SendMessage("DisableAI", 100);
            obj.gameObject.GetComponent<Enemy>().ModifiyHealth(damage / 10);
            //obj.gameObject.GetComponent<EnemyStats>().VisualizeDamage(obj);
           
            if(canKnockback == true)
            {
                AddKnockback();
            }
            else if(canLaunchUp == true)
            {
                AddKnockUp();
            }
            else
            {
                // For Later disable Ai 
            }
                
                
           obj.SendMessage("TakeDamage", damage / 10); obj.SendMessage("TakeDamage", damage / 10);

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
    #endregion
}

