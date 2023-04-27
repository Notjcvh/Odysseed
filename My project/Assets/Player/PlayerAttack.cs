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
    AggresiveKnockback,
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

    public float timeOfCharge;
    private int count = 0;
    public bool chargedAttack;

    private CameraController cam;
    private Quaternion targetRotation;
    public float chargedAttackMultiplier = 1.4f;


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
            else if (playerManger.IsGrounded() == true && playerManger.currentState == PlayerStates.Jumping) // if we land reset the combo 
            {
                isInAir = true;
                ResetCombo();
            }
        }
        #endregion
    }



    private void FixedUpdate()
    {
        if (canRotate == true && playerManger.MovementVector != Vector3.zero)
        {
          //  Debug.Log("Rotation");
            // if we have input that is either vertical or horizontal then is moving is true 
            playerManger.MovementVector = playerInput.movementInput;
            Vector3 cameraPlannerDirection = cam.CameraPlannerDirection;
            Quaternion cameraPlannerRotation = Quaternion.LookRotation(cameraPlannerDirection);
            //Aligning movement in relation to the camera
            playerManger.MovementVector = cameraPlannerRotation * playerManger.MovementVector;
            playerManger.TargetRot = Quaternion.LookRotation(playerManger.MovementVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10 * Time.fixedDeltaTime);
        }
    }
    #endregion


    #region Launch Attack

    public void LaunchAttack(int inputType)
    {
        animator.SetInteger("Mouse Input", inputType);
        animator.SetBool("isRunning", false);
        animator.SetBool("Attacking", true);
        animator.SetTrigger("Input Pressed");

        if (inputType <= 1)
        {
            // if the trigger is not set then the animation will not run
            // this also stops animation from looping in the air
            
        }
       
        Attack(inputType);
    }
    #endregion


    #region Attacks and Combos
    private void Attack(int inputType)
    {
        //Light Attacks
        if (inputType == 0)
        {
            switch (lightAttackCounter, playerManger.IsGrounded())
            {
                #region Ground Light Attacks
                case (0, true): // Starter
                    lightAttackCounter++;
                    animator.SetFloat("Starter Type", 0);
                    SetCombo(inputType, lightAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.None, 10, 5f, 5));
                    canRotate = true;
                    break;
                case (1, true):
                    lightAttackCounter++;
                    SetCombo(inputType, lightAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.None, 10, 5f, 5));
                    break;
                case (2, true):
                    lightAttackCounter++;
                    SetCombo(inputType, lightAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.AggresiveKnockback, 20, 5f, 20));
                    break;
                #endregion
                #region Air Light Attacks 
                case (0, false):
                    SetAttackBehaviour(0);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.AggresiveKnockback, 30, 5f, 50));
                    break;
                #endregion
                default:
                    break;
            }
        }
        else if(inputType == 1)
        {
            switch (heavyAttackCounter, playerManger.IsGrounded())
            {
                #region Ground Heavy Attacks
                case (0, true):
                     Debug.Log("Secondary Attack");
                     animator.SetFloat("Starter Type", 1);
                     heavyAttackCounter++;
                     SetCombo(inputType, heavyAttackCounter, 5f);
                     SendValues("Sword", new PlayerCollider(PhysicsBehaviours.None, 20, 3f, 1));
                    break;
                case (1, true):
                    heavyAttackCounter++;
                    SetCombo(inputType, heavyAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.None, 25, 5f, 1));
                    break;
                case (2, true):
                    heavyAttackCounter++;
                    SetCombo(inputType, heavyAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.KnockUp, 30, 5f, 10));
                    break;
                #endregion
                #region Air Heavy Attacks 
                case (0, false):
                    heavyAttackCounter++;
                    SetCombo(inputType, heavyAttackCounter, 5f);
                    SendValues("Sword", new PlayerCollider(PhysicsBehaviours.Knockdown, 20, 30, 10));
                    break;
                default:
                    break;
                #endregion
            }
        }
        else if(inputType == 2)
        {
            int attackRating = (int)Math.Ceiling(20 * chargedAttackMultiplier);
            SendValues("Sword", new PlayerCollider(PhysicsBehaviours.KnockUp, attackRating, 3f, 10));
            SetAttackBehaviour(2);
            ResetCombo();
        }
        else if( inputType == 3)
        {
            SetAttackBehaviour(3);
            ResetCombo();
        }

    }
    private void SetCombo(int inputType, int attacktype, float combolifetime)
    {
      //  audioController.PlayAudio(AudioType.PlayerAttack, false, 0, false);
        //increase the attack counters
        if (inputType == 0) // light attack 
            heavyAttackCounter++;
        else // heavy attack 
            lightAttackCounter++;

        animator.SetFloat("ComboLifetime", combolifetime);
        animator.SetInteger("Attack Type", attacktype);
        isAnimationActive = true;
    }

    private void SetAttackBehaviour(int inputTpye)
    {
        if (inputTpye == 0) //Light Air attack 
        {
            if(playerManger.superStates == SuperStates.Rising || playerManger.superStates == SuperStates.Falling)
            {
                playerManger.playerBody.velocity = Vector3.zero;
     
            }

            playerManger.playerBody.useGravity = false;
            //Apply Aggrasive Gravity
            StartCoroutine(playerMovement.ApplyGravity());
        }
        else if (inputTpye == 1) //Heavy Air Attack
        {
            //call functions
        }
        else if (inputTpye == 2)// Charge Attack
        {

        }
        else //Wave Attack
        {

        }
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
        playerManger.StopMovement = false;
        Debug.Log("Animation is Finished");
        animator.SetBool("Attacking", false);
        animator.ResetTrigger("Input Pressed");
        animator.ResetTrigger("LaunchChargedAttack");
        playerManger.isAttackAnimationActive = false;
      
        playerManger.isAttacking = false;
        canRotate = true;
        
        //Checking if the player has hit the combo finisher  
        if (lightAttackCounter == lightAttackMaxGround && playerManger.IsGrounded() == true || 
            heavyAttackCounter == heavyAttackMaxGround && playerManger.IsGrounded() == true ) // Finisher end of the Combo grounded
            ResetCombo();
        else if (lightAttackCounter == lightAttackMaxAir && playerManger.IsGrounded() != true)
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


    #region Sliding Forward When Attacking 
    public void SlideForward()
    {
        co = MoveForwardWhenAttacking(transform.position, lerpToPosition.position, lerpduration, playerManger.IsGrounded());
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

  

   
}

