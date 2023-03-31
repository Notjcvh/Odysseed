using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using UnityEditor.Animations; thomas coding is a dangerous thing that should only happen in extreme circumstances
using UnityEngine;
using Cinemachine;

public enum PhysicsBehaviours
{
    #region No Physics Behaviour
    None,
    #endregion

    #region Add a Single Physics Behaviour 
    Knockback,
    KnockUp,
    Knockdown,
    #endregion

    #region Add a Continous Physics Behaviour

    ContinousKnockback,
    #endregion
}


[RequireComponent(typeof(PlayerInput))]
public class PlayerAttack : MonoBehaviour
{
    //Handle Inputs, Button Clicks, Collisions, and Possible Physics Applying Knockback 
    [Header("Referencing")]
    public Transform playerPos;
    public Transform enmeyPosition;
    private Animator animator;
    private PlayerManger playerManger;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput;
    private AudioController audioController;

    [Header("Hit Detectetion")]
    public LayerMask whatIsHittable;

    [Header("Inputs")]
    public int inputType;
    public float chargeSpeed;
    public float chargeTime;
    public bool chargedAttack;

    private CameraController cam;
    private Quaternion targetRotation;
   


    [Header("States")]
    public bool isInAir;
    public bool isAnimationActive;
    public bool canRotate = true;

    [Header("In Combo")]
    // These are the counters that will be set to the animator Attack Type parameter
    public int lightAttackCounter;
    public int heavyAttackCounter;

    // Set these to the amount states(light or heavy) in the ground or air or charged attack strings 
    [SerializeField] private int lightAttackMaxGround;[SerializeField] private int lightAttackMaxAir;
    [SerializeField] private int heavyAttackMaxGround;[SerializeField] private int heavyAttackMaxAir; 
    public float comboLifeCounter = 0;
    [Range(0, 5)] public float animMultiplier;

    [Header("Attack Behaviours")]
    //Does Merlot Slide foreward ?
    private Transform lerpToPosition;
    public float lerpduration;
    private LayerMask playerCollionMask;
    IEnumerator co;

    [Header("List of Colliders")]
    public Dictionary<string, HitCollider> colliders = new Dictionary<string, HitCollider>();
    public bool activateCollidersAcive;

    public class PlayerCollider
    {
        public Transform origin;
       // public LayerMask whatIsHittable;
        public PhysicsBehaviours behaviours;
        public int damage;
        public float timer;
        public float strength;

        public PlayerCollider(PhysicsBehaviours behaviourType, int _damage, float _timer, float _strength)
        {
          //  whatIsHittable = layers;
            behaviours = behaviourType;
            damage = _damage;
            timer = _timer;
            strength = _strength;
        }
    }

    #region Unity Functions
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        cam = GetComponent<CameraController>();
        playerInput = GetComponent<PlayerInput>();
        playerManger = GetComponent<PlayerManger>();
        animator = GetComponentInChildren<Animator>();
        audioController = GetComponent<AudioController>();
    }

    void Update()
    {
      
        #region Handling Mouse Inputs
        if (playerInput.attack && isAnimationActive == false)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("Attacking", true);
            inputType = 0; // 0 represents the left mouse button 
            animator.SetInteger("Mouse Input", inputType);

            // if the trigger is not set then the animation will not run
            // this also stops animation from looping in the air
            animator.SetTrigger("Input Pressed");
            Attack(inputType);
        }
        else if (playerInput.secondaryAttack && chargeTime <= 1.5 && isAnimationActive == false)
        {
           
            animator.SetBool("isRunning", false);
            animator.SetBool("Attacking", true);
            inputType = 1; //1 represents the right mouse button 
            animator.SetInteger("Mouse Input", inputType);
            animator.SetTrigger("Input Pressed");
            Attack(inputType);
            chargeTime = 0;
            chargedAttack = false;
        }

        if (playerInput.chargedSecondaryAttack && comboLifeCounter <= 0)
        {
            Debug.Log("Charging");
            bool isCharging = true;
            if(isCharging == true)
            {
                chargeTime += Time.deltaTime;
            }
            if(chargeTime > 1.7)
            {
                Rotate();
            }
            if(chargeTime > 2)
            {
                chargedAttack = true;
            }
        }

        if(chargedAttack == true && !playerInput.chargedSecondaryAttack && isAnimationActive == false)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("Attacking", true);
            inputType = 1; //1 represents the right mouse button 
            animator.SetInteger("Mouse Input", inputType);
            animator.SetTrigger("Input Pressed");
            chargeTime = 0;
            Attack(inputType);
            chargedAttack = false;
        }
        #endregion

        if (isAnimationActive == true)
        {
            playerMovement.stopMovementEvent = true;
            Rotate();
        }
        else
            playerMovement.stopMovementEvent = false;





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

        if(comboLifeCounter > 0)
        {
            if (playerManger.currentState == PlayerStates.Moving)
            {
                ResetCombo();
            }
            else if (playerManger.currentState == PlayerStates.Jumping) // if we jump reset combo 
            {
                isInAir = true;
                ResetCombo();
            }
            else if (playerMovement.IsGrounded() == true && playerManger.currentState == PlayerStates.Jumping) // if we land reset the combo 
            {
                isInAir = true;
                ResetCombo();
            }
        }
        #endregion


        if(playerMovement.isDashing == true)
        {
            animator.SetBool("Attacking", false);
            isAnimationActive = false;
        }
    }
    #endregion

    #region Attacks and Combos
    private void Attack(int inputType)
    {
       
        playerManger.currentState = PlayerStates.Attacking;
        audioController.PlayAudio(AudioType.MainMenu, true, 0, false);


       


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
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.None, 20, 5f, 10));
                    break;
                case (1, true):
                    lightAttackCounter++;
                    Set(inputType, lightAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.None, 20, 5f, 10));
                    break;
                case (2, true):
                    lightAttackCounter++;
                    Set(inputType, lightAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.None, 20, 5f, 10));
                    break;
                #endregion
                #region Air Light Attacks 
                case (0, false):
                    lightAttackCounter++;
                    Set(inputType, lightAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.Knockback, 20, 5f, 10));
                    break;
                #endregion
                default:
                    break;
            }
        }
        else
        {
            switch (heavyAttackCounter, playerMovement.IsGrounded(),chargedAttack)
            {
                #region Ground Heavy Attacks
                case (0, true, false):
                     Debug.Log("Secondary Attack");
                     animator.SetFloat("Starter Type", 1);
                     heavyAttackCounter++;
                     Set(inputType, heavyAttackCounter, 5f);
                    // SendValues("Sword", new PlayerCollider(PhysicsBehaviours.KnockUp, 20, 3f, 5));
                    break;
                case (1, true, false):
                    heavyAttackCounter++;
                    Set(inputType, heavyAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.None, 20, 5f, 10));
                    break;
                case (2, true, false):
                    heavyAttackCounter++;
                    Set(inputType, heavyAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.None, 20, 5f, 10));
                    break;
                #endregion

                #region Charged Starter
                case (0, true, true):
                    Debug.Log("Charged Attack");
                    animator.SetFloat("Starter Type", 2);
                    heavyAttackCounter++;
                    Set(inputType, heavyAttackCounter, 0.1f);
                    break;

                #endregion

                #region Air Heavy Attacks 
                case (0, false, false):
                    heavyAttackCounter++;
                    Set(inputType, heavyAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.Knockdown, 20, 30, 10));
                    break;
                default:
                    break;
            }
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

    private void SendValues(string myCollider, PlayerCollider values)
    {
        if(colliders.ContainsKey(myCollider))
        {
            HitCollider calledCollider = colliders[myCollider];
            calledCollider.MyBehaviour(values);
        }
    }
    #endregion

    //This is called by the animation trigger
    public void isAnimationFinished()
    {
        Debug.Log("Animation is Finished");
        animator.SetBool("Attacking", false);
        animator.ResetTrigger("Input Pressed");
        isAnimationActive = false;
        canRotate = true;
        
        //Checking if the player has hit the combo finisher  
        if (lightAttackCounter == lightAttackMaxGround && playerMovement.IsGrounded() == true || 
            heavyAttackCounter == heavyAttackMaxGround && playerMovement.IsGrounded() == true ) // Finisher end of the Combo grounded
            ResetCombo();
        else if (lightAttackCounter == lightAttackMaxAir && playerMovement.IsGrounded() != true)
            ResetCombo();
        else
            comboLifeCounter = animator.GetFloat("ComboLifetime");
    }

    public void ResetCombo()
    {
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

    #region Sliding Forward When Attacking 
    public void SlideForward()
    {
        co = MoveForwardWhenAttacking(transform.position, lerpToPosition.position, lerpduration, playerMovement.IsGrounded());
        StartCoroutine(co);
    }
    private IEnumerator MoveForwardWhenAttacking(Vector3 currentPostion, Vector3 endPosition, float time, bool isGrounded)
    {
        if (isGrounded == false)
            yield return null;
        else
        {
         // RaycastHit hit;
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
   

    public void StopAnimRotation()
    {
        canRotate = false;
    }

    void Rotate()
    {
        if (canRotate == true)
        {
            Vector3 cameraPlannerDirection = cam.CameraPlannerDirection;
            Quaternion cameraPlannerRotation = Quaternion.LookRotation(cameraPlannerDirection);
            targetRotation = cameraPlannerRotation;
            Quaternion lerp = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 10);
            transform.rotation = lerp;
        }
    }
}

