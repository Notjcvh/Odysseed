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


    [Header("Movement")]
    private bool isMoving;
    public Vector3 newVelocity;
    public Vector3 rbRefVelocity;
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
    public float currentJumpTime = 0; //the current amount of time passed since the first frame of the jump 
    public float fallMultiplier = 2.3f;
    public float lowFallMultiplier = 1f;

    /*
     public float maxJumpHeight = 1;
     public float maxJumpTime2 = 2f;
     public float maxJumpHeight2= 1;
     public AnimationCurve jumpCurve;
     public float jumpHieghtMultiplier = 2; // applied to jump force based on how long the button is held down for
     public float gravity; // our accleration 
     public float time;
     public AnimationCurve gravity;
     */
    
    // Possibly will be removed 
    [SerializeField] private float verticalVelocity;
    [SerializeField] private float gravitySpeed;
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
    private int numberOfDashes = 2;
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
        CheckForSpecialGravity();

        playerBody.drag = 7;

        currentAnimation = animator.GetCurrentAnimatorStateInfo(0).ToString();

        #region Locomotion Inputs
        if (playerInput.movementInput != Vector3.zero && stopMovementEvent != true ) // The player is moving 
        {
            CanMove();
            animator.SetBool("isRunning", true);
            playerManger.currentState = PlayerStates.Moving;
        }
        else // The player is Idle 
        {
            newVelocity.y = 0;
            playerManger.currentState = PlayerStates.Idle;
            animator.SetBool("isRunning", false);
            isMoving = false;
        }

        //

//   else if (playerInput.jumpInput && IsGrounded() || playerInput.jumpInput && IsGrounded() == false && currentJumpTime <= maxJumpTim


        if (IsGrounded() == true)
        {

            animator.SetBool("isGrounded", IsGrounded());
            if (currentJumpTime <= maxJumpTime && playerInput.jumpInput)
            {
                isJumping = true;
                currentJumpTime += Time.fixedDeltaTime;
                CalculateJump();
                //Start Jump
            }

            else if (currentJumpTime >= maxJumpTime && !playerInput.jumpInput)
            {
                currentJumpTime = 0;

            }

            else if(currentJumpTime > 0 && isFalling == true)
            {
                currentJumpTime = 0;
            }
            isFalling = false;
       
        }
        else
        {
            animator.SetBool("isGrounded", IsGrounded());
            if (currentJumpTime <= maxJumpTime && playerInput.jumpInput)
            {
                isJumping = true;
                currentJumpTime += Time.fixedDeltaTime;
                CalculateJump();
                //Contimue Jump
            }
            else if(currentJumpTime <= maxJumpTime && !playerInput.jumpInput)
            {
                isJumping = false;

            }
            else
            {
                isFalling = true;
                isJumping = false;
                //We can no longer jump so we are falling
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

         

    }


    #region Moving the Player
    private void CanMove()
    {
        // if we have input that is either vertical or horizontal then is moving is true 
        movementVector = playerInput.movementInput;
        Vector3 cameraPlannerDirection = cam.CameraPlannerDirection;
        Quaternion cameraPlannerRotation = Quaternion.LookRotation(cameraPlannerDirection);
        //Aligning movement in relation to the camera
        movementVector = cameraPlannerRotation * movementVector;
        // checking for inputs from Player
        targetSpeed = movementVector != Vector3.zero ? runSpeed : 0;
        newVelocity = movementVector * targetSpeed;
        newVelocity.y = 0;
        playerBody.velocity = newVelocity;

        isMoving = true;
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
    private void CalculateJump()
    {
        float jumpApex = maxJumpTime / 2;
        if (currentJumpTime <= jumpApex && playerBody.velocity.x == 0 && playerBody.velocity.z ==0)
            playerBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        else if(currentJumpTime <= jumpApex && isMoving == true)
        {
            //add movement vector and vector 3.zero 
            //Vector3 c = new Vector3(movementVector.x + 0, movementVector.y + 1, movementVector.z + 0);
            //  float angle = Vector3.Angle(movementVector, c); // should co
            //    float rad = angle * Mathf.Deg2Rad;

            Vector3 direction = movementVector + (Vector3.up * 20);
            //new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), Mathf.Cos(rad));

            playerBody.AddForce( direction.normalized * JumpForce * 10, ForceMode.Impulse);
        }
            /*
            //Jump 1
            //caluclate basic gravity based of time duration 
            if (playerBody.velocity.y == 0)
            {
                maxJumpTime = 1;
                float jumpApex = maxJumpTime / 2;

              //  gravity = (2 * maxJumpHeight) / Mathf.Abs(jumpApex * jumpApex); //-40

                if (currentJumpTime <= jumpApex)
                {
                    float intialjumpVelocity = (2 * maxJumpHeight) / jumpApex; //20
                    verticalVelocity = Mathf.Abs(intialjumpVelocity);
                    Vector3 newJumpVelocity = new Vector3(0, verticalVelocity, 0);
                    playerBody.AddForce(newJumpVelocity, ForceMode.Impulse);

                }
            }*/

            //Calculate Jump 2 with respect to horizontal velocity (Projectile Motion  and Freefall)
/*

            float intialHor = movementVector.magnitude * Mathf.Cos(angle);
            float intialVert = Vector3.up.magnitude * Mathf.Sin(angle);

           
            float vertDisplacement = intialVert * maxJumpTime2;

       
        }*/
    }
    #endregion


    private void CheckForSpecialGravity()
    {
        if (playerBody.velocity != Vector3.zero && !playerInput.jumpInput)
        {
            playerBody.AddForce(Vector3.down * JumpForce * 10 * (fallMultiplier - 1), ForceMode.Acceleration);
   
            /*
            float previousYVelocity = playerBody.velocity.y;
            gravitySpeed += gravity * Time.fixedDeltaTime; 
            Vector3 gravityForce = new Vector3(0, gravitySpeed, 0);
            playerBody.AddForce(gravityForce);*/
        }
        /*
        else if(IsGrounded() != true && targetSpeed != 0)
        {
            //Double Gravity
            gravitySpeed += (gravity * Time.fixedDeltaTime)*20;
            Vector3 gravityForce = new Vector3(0, gravitySpeed, 0);
            playerBody.AddForce(gravityForce);
        }*/
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




