using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    private CameraController cam;
    private PlayerInput playerInput;
    private PlayerManger playerManger;
    [SerializeField] private Rigidbody playerBody;
  
    [SerializeField] private Transform lungePosition;
    [SerializeField] private Transform distanceToGround;
    private Animator animator;

    public AudioController audioController;

    public Vector3 newVelocity; // new setter 
    public Vector3 rbRefVelocity; // calculation passer 
 

    [Header("Movement")]
    private bool isMoving;
    private Vector3 movementVector;
    private Quaternion targetRotation;
    public float targetSpeed;
    public float runSpeed;
    public bool stopMovementEvent;
    public bool isTalking = false;
    public bool isRestricted = false;

    [Header("Jumping")]
    public bool isFalling;
    public bool isJumping;
    public float JumpForce;
    public LayerMask Ground;
    private float maxJumpTime = .5f; // the max time for the jump to complete, from standstill to standstill 
    private float jumpApex;
    public float currentJumpTime = 0; //the current amount of time passed since the first frame of the jump 
    public float fallMultiplier = 2.3f;
    public float lowFallMultiplier = 1f;

    /*
     public float maxJumpHeight = 1;
     public float maxJumpTime2 = 2f;
     public float maxJumpHeight2= 1;
     public AnimationCurve jumpCurve;
     public float jumpHieghtMultiplier = 2; // applied to jump force based on how long the button is held down for
   
     public float time;
     public AnimationCurve gravity;
     */
    
    // Possibly will be removed 
    [SerializeField] private float verticalVelocity;
    [SerializeField] private float gravitySpeed;
    public float gravity; // our accleration 
    public Vector3 playerVerticalVelocity;


    [Header("Dash")]
    public bool isDashing = false;
    public Transform lerpToPosition;
    public float dashStartValue;
    public AnimationCurve dashValueCurve;
    public float dashAnimationLifetime = 0;
    public float force;
    private float dashAnimationEndTime;
    private bool incremented = false;
  //  private int numberOfDashes = 2;
    public int numberOfUsedDashes;
    public float dashResetTimer = 0;
    public float lerpduration;
    public LayerMask playerCollionMask;

    [Header("Targeting")]
    public bool targetingEnemy;
    public Transform target;
    public float range;

    [Header("Seeds")]
    public int seedId;

    [Header("Animation States")] // Right these are for playing animations not conected to transitions in the animator 
    const string Player_Jump = "Player_Jump";
    const string Player_Dash = "Player_Dash";


    private string currentAnimation;
    private void Awake()
    {
        cam = GetComponent<CameraController>();
        playerInput = GetComponent<PlayerInput>();
        playerBody = GetComponentInChildren<Rigidbody>();
        playerManger = GetComponent<PlayerManger>();
        animator = GetComponent<PlayerManger>().animator;
        stopMovementEvent = !stopMovementEvent; //negating the bool value to invert the value of true and false 


        //We want to make sure when the game starters the animator recognizes the player is on the groun
        animator.SetBool("isGrounded", IsGrounded());



    }

    private void Update()
    {

        if (isDashing == false && incremented == true)
        {
            // start to reset values
            dashResetTimer += 1 * Time.deltaTime;
        }

        if(dashResetTimer >= 1)
        {
            incremented = false;
            Debug.Log("Can Dash Again");
            dashAnimationLifetime = 0;
            force = dashValueCurve.Evaluate(0);
            dashAnimationLifetime = 0;
            dashResetTimer = 0;

        }

    }
    private void FixedUpdate()
    {
        
        //Apply
        playerBody.drag = 7;
        currentAnimation = animator.GetCurrentAnimatorStateInfo(0).ToString();
        rbRefVelocity = playerBody.velocity; //reference velocity
        jumpApex = maxJumpTime / 2;
    

        // create a function to set velocity at one place

        #region Locomotion Inputs
        if (playerInput.movementInput != Vector3.zero && stopMovementEvent != true ) // The player is moving 
        {
            CanMove(ref rbRefVelocity);
            animator.SetBool("isRunning", true);
            playerManger.currentState = PlayerStates.Moving;
        }
        else // The player is Idle 
        {
            newVelocity.y = 0;
            playerManger.currentState = PlayerStates.Idle;
            animator.SetBool("isRunning", false);
            isMoving = false;
            rbRefVelocity = Vector3.zero;
            SetVelocity(playerBody.velocity, rbRefVelocity);
        }
        //   else if (playerInput.jumpInput && IsGrounded() || playerInput.jumpInput && IsGrounded() == false && currentJumpTime <= maxJumpTim

        if (IsGrounded() == true)
        {
            animator.SetBool("isGrounded", IsGrounded());
            if (playerInput.jumpInput && currentJumpTime <= jumpApex)
            {
                isJumping = true;
                CalculateJump(ref rbRefVelocity);
                Debug.Log("start jumping");
            }

            if(isFalling == true)
            {
                currentJumpTime = 0;
                isFalling = false;
            }
        }
        else 
        {          
            gravity = (-2 * maxJumpTime) / ( jumpApex * jumpApex);
            animator.SetBool("isGrounded", IsGrounded());
            if (playerInput.jumpInput && currentJumpTime <= jumpApex)
            {
                isJumping = true;
                currentJumpTime += Time.fixedDeltaTime;
                CalculateJump(ref rbRefVelocity);
                Debug.Log("continue jumping");
                //Contimue Jump
            }
        } 
      
        #region Dashing
        //Dashing
        if (playerInput.dash && dashAnimationLifetime <= dashAnimationEndTime)
        {
            if (!incremented)
            {
              incremented = true;
            }

            Debug.Log("Dash");
            //change values within the animator
            isDashing = true;
            stopMovementEvent = true;
            animator.SetBool("isDashing", isDashing);
            ChangeAnimationState(Player_Dash);
            playerManger.currentState = PlayerStates.Dashing;

            //convert animation cuvrve to the length of the animation
            AnimatorClipInfo[] clip = animator.GetCurrentAnimatorClipInfo(0);
            Keyframe[] keys = dashValueCurve.keys;
            Keyframe lastKey = keys[keys.Length - 1];
            
            float end = lastKey.time;

            //Setting to the end of the animation 
            dashAnimationEndTime = end;

            //Call the  Dash function
            Dash(dashAnimationLifetime);
            dashAnimationLifetime += Time.fixedDeltaTime;
        }
        else
        {
            isDashing = false;
            animator.SetBool("isDashing", isDashing);
            stopMovementEvent = false;
        }
        #endregion

        #endregion
        //Enemy Targetting 
        if (playerInput.target)
            UpdateTarget();
        if (targetingEnemy)
            this.gameObject.transform.LookAt(target);

        CheckForSpecialGravity(ref rbRefVelocity);

        SetVelocity(playerBody.velocity, rbRefVelocity);

    }

    private void SetVelocity(Vector3 rigibodyVelocity, Vector3 additionalVelocity)
    {
         playerBody.velocity += additionalVelocity;

        if(playerBody.velocity.magnitude > targetSpeed)
        {

            playerBody.velocity = Vector3.ClampMagnitude(playerBody.velocity, targetSpeed);
            
        }

       
    }


    #region Moving the Player
    private void CanMove(ref Vector3 currentVelocity)
    {
        isMoving = true;
        // if we have input that is either vertical or horizontal then is moving is true 
        movementVector = playerInput.movementInput;
        Vector3 cameraPlannerDirection = cam.CameraPlannerDirection;
        Quaternion cameraPlannerRotation = Quaternion.LookRotation(cameraPlannerDirection);
        //Aligning movement in relation to the camera
        movementVector = cameraPlannerRotation * movementVector;
        // checking for inputs from Player
        targetSpeed = movementVector != Vector3.zero ? runSpeed : 0;
        currentVelocity = movementVector * targetSpeed;
        currentVelocity.y = 0;     
        if (targetSpeed != 0 && isDashing == false)
        {
            targetRotation = Quaternion.LookRotation(movementVector);
            transform.rotation = targetRotation;
        }
    }
    #endregion

    #region Ground Check
    public bool IsGrounded()
    {
        Vector3 direction = Vector3.down;
        RaycastHit hit;
        if (Physics.Raycast(distanceToGround.position, direction, out hit, .1f, Ground))
            return true;
        else
            return false;
    }
    #endregion

    #region Jumping     
    private void CalculateJump(ref Vector3 velocity)
    {
        float jumpApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpTime) / (jumpApex * jumpApex);
        //Stationary Jump 
        float verticalMagnitude = (2 * maxJumpTime) / jumpApex;
        velocity = Vector3.up * verticalMagnitude;
        // if (isMoving == false && currentJumpTime <= maxJumpTime)
      
        /* Parabolic Jump 
        else
        {
            float angle = Vector3.Angle(velocity.normalized, Vector3.up) / 2; // 45 degrees

            Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);

            float Vo = (2 * maxJumpTime * horizontalVelocity.magnitude) / (jumpApex * jumpApex);

            float initialHor = Vo * Mathf.Cos(angle);
            float initalVert = Vo * Mathf.Sin(angle);

            gravity = (-2 * maxJumpTime * (initialHor * initialHor)) / (jumpApex * jumpApex);

            float verticalDisplacement = initalVert * jumpApex;

            velocity.y += verticalDisplacement;
        }*/
    }
    #endregion

    private void CheckForSpecialGravity(ref Vector3 velocity)
    {
        if (IsGrounded() == false)
        {
            if(isMoving != true)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
                isFalling = true;
            }

            else
            {
                velocity.y += gravity * Time.fixedDeltaTime * 10;
                isFalling = true;
            }
          


        }

    
    }

    #region Dashing

    void Dash(float time)
    {
        force = dashValueCurve.Evaluate(time);
        if (movementVector == Vector3.zero)
            playerBody.AddForce(transform.forward * force * Time.fixedDeltaTime, ForceMode.VelocityChange);
        else
            playerBody.AddForce(movementVector * force * Time.fixedDeltaTime, ForceMode.VelocityChange);
      //  print(force);
    }
    #endregion

    #region Player Targeting 
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        foreach (GameObject enemy in enemies)
        {
            Vector3 TargetScreenPoint = Camera.main.WorldToScreenPoint(enemy.transform.position);
            
            float distance = Vector2.Distance(TargetScreenPoint, screenCenter);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            nearestEnemy.GetComponent<Enemy>().isTargeted = true;
        }

    }
    #endregion

    #region Change Animation State
    void ChangeAnimationState(string triggerAnimation)
    {
        //print(triggerAnimation);
        if (currentAnimation == triggerAnimation) return;

        animator.Play(triggerAnimation);

        currentAnimation = triggerAnimation;
    }
    #endregion


    private void OnDrawGizmos()
    {
        float range = 2f;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * range));

        Gizmos.color = Color.black;
        //add movement vector and vector 3.zero 
        Vector3 c = new Vector3(movementVector.x + 0, movementVector.y + 1, movementVector.z + 0);
        /*Gizmos.DrawRay(distanceToGround.position, c.normalized * 10);
        if (currentJumpTime <= maxJumpTime)
        {
            //calculate a direction from ground
            Vector3 direction = Vector3.Normalize(playerBody.position - distanceToGround.position);

            float multiplier = jumpCurve.Evaluate(currentJumpTime / (maxJumpTime * jumpHieghtMultiplier));

            Vector3 jumpVector = new Vector3(0, direction.y * multiplier * jumpForce, 0);

            Gizmos.DrawRay(jumpVector, direction);

            //playerBody.AddForce(jumpVector, ForceMode.Impulse);*/

    }   
}




