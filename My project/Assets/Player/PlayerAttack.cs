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
    public Transform playerPos;
    public Transform enmeyPosition;
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput;

    [Header("Hit Detectetion")]
    public Collider attackArea;
    public Transform attackPosition;
    public LayerMask whatIsHittable;
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

    [Header("Knockback or Launch Up")]
    public float knockbackTimer;
    public float knockbackStrength;
    public bool canKnockback;
    public bool canLaunchUp;

    [Header("States")]
    public bool isInAir;
    public bool isAnimationActive;


    [Header("In Combo")]
    // These are the counters that will be set to the animator Attack Type parameter
    public int lightAttackCounter;
    public int heavyAttackCounter;

    // Set these to the amount states(light or heavy) in the ground or air attack strings 
    [SerializeField] private int lightAttackMaxGround;[SerializeField] private int lightAttackMaxAir;
    [SerializeField] private int heavyAttackMaxGround;[SerializeField] private int heavyAttackMaxAir;

    public float comboLifeCounter = 0;
    [Range(0, 10)] public float animMultiplier;



    #region Unity Functions
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (obj != null)
            direction = (obj.transform.position - attackPosition.position).normalized; // finding the direction from attackPos to Obj rigidbody. In update so Knockback happen for the full time between frames

        #region Handling Mouse Inputs
        if (playerInput.attack && isAnimationActive == false)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("Attacking", true);
            inputType = 0; // 0 represents the left mouse button 
            animator.SetInteger("Mouse Input", inputType);

            // if the trigger is not set then the animation will not run
            // this also stops animation from looping in the air
            animator.SetTrigger("Input Pressed");
            Attack(inputType);
        }
        else if (playerInput.secondaryAttack && isAnimationActive == false)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("Attacking", true);
            inputType = 1; //1 represents the roght mouse button 
            animator.SetInteger("Mouse Input", inputType);
            animator.SetTrigger("Input Pressed");
            Attack(inputType);
        }
        #endregion

        #region Handeling Starting and Ending Combo
        if (comboLifeCounter > 0)  //begin countdown 
        {
            comboLifeCounter -= 1 * Time.deltaTime;
        }
        else if (comboLifeCounter < 0) // Combo is dropped call Reset Combo
        {
            animator.SetFloat("ComboLifetime", comboLifeCounter);
            ResetCombo();
        }
        #endregion

        #region Handeling Special Case Resets
        //if we move, jump, or dash the combo counter will reset 
        // this is assuming the player wants to reset intentionally 
        if (playerMovement.targetSpeed != 0 && comboLifeCounter > 0)
            ResetCombo();
        else if (playerMovement.IsGrounded() && playerMovement.Jump()) // if we jump reset combo 
        {
            isInAir = true;
            animator.SetBool("isGrounded", playerMovement.IsGrounded());
            ResetCombo();
        }
        else if (isInAir == true && playerMovement.playerVerticalVelocity == Vector3.zero) // if we land reset the combo 
        {
            isInAir = false;
            ResetCombo();
        }
        #endregion
    }
    #endregion

    #region Attacks and Combos
    private void Attack(int inputType)
    {
        playerMovement.stopMovementEvent = true;

        //Light Attacks
        if (inputType == 0)
        {
            switch (lightAttackCounter, playerMovement.IsGrounded())
            {
                #region Ground Light Attacks
                case (0, true): // Starter
                    lightAttackCounter++;
                    animator.SetFloat("Starter Type", 0);
                    Set(inputType, lightAttackCounter, 5f);
                    break;
                case (1, true):
                    lightAttackCounter++;
                    Set(inputType, lightAttackCounter, 5f);
                    break;
                case (2, true):
                    lightAttackCounter++;
                    Set(inputType, lightAttackCounter, 5f);
                    canKnockback = true;
                    break;
                #endregion
                #region Air Light Attacks 
                case (0, false):
                    lightAttackCounter++;
                    Set(inputType, lightAttackCounter, 5f);
                    break;
                case (1, false):
                    lightAttackCounter++;
                    Set(inputType, lightAttackCounter, 5f);
                    break;
                #endregion
                default:
                    break;
            }
            OnTriggerEnter(attackArea);
            // StartCoroutine(DelayAttack()); // For Later what does this do
        }
        else
        {
            switch (heavyAttackCounter, playerMovement.IsGrounded())
            {
                #region Ground Heavy Attacks
                case (0, true):
                    animator.SetFloat("Starter Type", 1);
                    heavyAttackCounter++;
                    Set(inputType, heavyAttackCounter, 5f);
                    break;
                case (1, true):
                    heavyAttackCounter++;
                    Set(inputType, heavyAttackCounter, 5f);
                    break;
                case (2, true):
                    heavyAttackCounter++;
                    Set(inputType, heavyAttackCounter, 5f);
                    break;
                #endregion
                #region Air Heavy Attacks 
                case (0, false):
                    heavyAttackCounter++;
                    Set(inputType, heavyAttackCounter, 5f);
                    break;
                case (1, false):
                    heavyAttackCounter++;
                    Set(inputType, heavyAttackCounter, 5f);
                    break;
                #endregion
                default:
                    break;
            }
            OnTriggerEnter(attackArea);
        }
    }

    private void Set(int inputType, int attacktype, float combolifetime)
    {
        //increase the attack counters
        if (inputType == 0) // light attack 
            heavyAttackCounter++;
        else // heavy attack 
            lightAttackCounter++;


        animator.SetFloat("ComboLifetime", combolifetime);
        animator.SetInteger("Attack Type", attacktype);
        isAnimationActive = true;
    }

    //This is called by the animation trigger
    public void isAnimationFinished()
    {
        Debug.Log("Finished");
        playerMovement.stopMovementEvent = false;
        animator.SetBool("Attacking", false);
        animator.ResetTrigger("Input Pressed");
        isAnimationActive = false;

        //Checking if the player has hit the combo finisher  
        if (lightAttackCounter == lightAttackMaxGround && playerMovement.IsGrounded() == true || heavyAttackCounter == heavyAttackMaxGround && playerMovement.IsGrounded() == true) // Finisher end of the Combo grounded
            ResetCombo();
        else if (lightAttackCounter == lightAttackMaxAir && playerMovement.IsGrounded() != true)
            ResetCombo();
        else
            comboLifeCounter = animator.GetFloat("ComboLifetime");
    }

    public void ResetCombo()
    {
        Debug.Log("ran");
        lightAttackCounter = 0;
        heavyAttackCounter = 0;
        animator.SetInteger("Attack Type", 0);
        isAnimationActive = false;
        animator.SetBool("Attacking", false);
        animator.ResetTrigger("Input Pressed");
        comboLifeCounter = -1; // we set it to -1 so the animator contion : Combolifetime can understand that we are dropping the combo 
        animator.SetFloat("ComboLifetime", comboLifeCounter);
        comboLifeCounter = 0;
    }
    #endregion

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

            if (canKnockback == true)
            {
                AddKnockback();
            }
            else if (canLaunchUp == true)
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
        co = MoveForwardWhenAttacking(transform.position, lerpPosition.position, lerpduration, playerMovement.IsGrounded());
        StartCoroutine(co);
    }
    private IEnumerator MoveForwardWhenAttacking(Vector3 currentPostion, Vector3 endPosition, float time, bool isGrounded)
    {
        if (isGrounded == false)
            yield return null;
        else
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
    }
    #endregion
  
}

