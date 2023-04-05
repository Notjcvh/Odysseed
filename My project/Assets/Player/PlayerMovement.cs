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
    private Animator animator;
    public AudioController audioController;
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Transform distanceToGround;


    public Vector3 newVelocity; // new setter 
    public Vector3 rbRefVelocity; // calculation passer 
 
    [Header("Movement")]
    private bool isMoving;
    public Vector3 movementVector;
    private Quaternion targetRotation;
    public float targetSpeed;
    public bool stopMovementEvent;
    public bool isTalking = false;
    public bool isRestricted = false;

    [Header("Jumping")]
    public bool isFalling;
    public bool isJumping;
    private float gravity;
    public AnimationCurve gravityValueCurve;
    public float gravityMultiplier = 0;
    private IEnumerator jumpCorutine;
    public LayerMask Ground;
    public float lastPlayerPosY;
    public float jumpTime;
    public float jumpForce;
    public float jumpElapsedTime;


    [Header("Dash")]
    public bool isDashing = false;
    public AnimationCurve dashValueCurve;
    public float force;
  //  private int numberOfDashes = 2;
    public int numberOfUsedDashes;
    public float dashResetTimer = 0;
    public float lerpduration;
    private IEnumerator dashCorutine;

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


        Keyframe[] keys = gravityValueCurve.keys;
        Keyframe lastKey = keys[keys.Length - 1];
        float end = lastKey.time;
        jumpTime = end;


        //We want to make sure when the game starters the animator recognizes the player is on the groun
        animator.SetBool("isGrounded", IsGrounded());
    }


    private void Update()
    {
      //  print(stopMovementEvent);

        if (playerInput.movementInput != Vector3.zero && stopMovementEvent != true) // The player is moving 
        {
            isMoving = true;
            playerManger.currentState = PlayerStates.Moving;
        }
        else
        {
            isMoving = false;
            playerManger.currentState = PlayerStates.Idle;
        }
        //Dashing
        if (playerInput.dash && isDashing == false)
        {
            isDashing = true;
            Debug.Log("Dash");
            playerManger.currentState = PlayerStates.Dashing;
            stopMovementEvent = true;
            animator.SetBool("isDashing", isDashing);
            stopMovementEvent = true;
        }





        // create a function to set velocity at one place
        #region Locomotion Inputs
  
        if (IsGrounded() == true)
        {
            animator.SetBool("isGrounded", IsGrounded());
            jumpElapsedTime = 0;
        }
        else
        {
            animator.SetBool("isGrounded", IsGrounded());
            isFalling = true;
            animator.SetBool("isFalling", isFalling);
        }

        if (playerInput.jumpInput && IsGrounded() == true)
        {
            isJumping = true;
            animator.SetBool("isJumping", isJumping);
            playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }


        checkYVelocity();

        #endregion
        //Enemy Targetting 
        if (playerInput.target)
            UpdateTarget();
        if (targetingEnemy)
            this.gameObject.transform.LookAt(target);
    }

    private void FixedUpdate()
    {
        if (isMoving == true)
        {
            CanMove();
        }
        else
        {
            playerManger.currentState = PlayerStates.Idle;
            animator.SetBool("isRunning", false);
            isMoving = false;
        }      
       
        if(isJumping == true)
        {
            jumpCorutine = ApplyGravity(jumpTime);
            StartCoroutine(jumpCorutine);
        }

        if (isDashing == true)
        {
            Keyframe[] keys = dashValueCurve.keys;
            Keyframe lastKey = keys[keys.Length - 1];
            float end = lastKey.time;
            //Setting to the end of the animation 
            float dashAnimationEndTime = end;
            dashCorutine = Dash(dashAnimationEndTime);
            StartCoroutine(dashCorutine);
        }
        else
        {
          
        }
    }

    private void checkYVelocity()
    {
        if(transform.hasChanged)
        {
            // Set blend tree threasholds
            float start = transform.position.y;
            float jumpApexTime = jumpTime/2;          
            if (transform.position.y > lastPlayerPosY && IsGrounded() == false)
            {
               animator.SetFloat("PlayerYVelocity", playerBody.velocity.y);
            }
            else if (transform.position.y < lastPlayerPosY)
            {
                animator.SetFloat("PlayerYVelocity", playerBody.velocity.y);
            }
            lastPlayerPosY = transform.position.y;
        }     
    }

    #region Moving and Idle
    private void CanMove()
    {
        // if we have input that is either vertical or horizontal then is moving is true 
        movementVector = playerInput.movementInput;
        Vector3 cameraPlannerDirection = cam.CameraPlannerDirection;
        Quaternion cameraPlannerRotation = Quaternion.LookRotation(cameraPlannerDirection);
        //Aligning movement in relation to the camera
        movementVector = cameraPlannerRotation * movementVector;
        
        // checking for inputs from Player
        float speed = movementVector != Vector3.zero ? targetSpeed : 0;

        playerBody.MovePosition(transform.position + movementVector * Time.deltaTime * speed);
        if (speed != 0 && isDashing == false)
        {
            animator.SetBool("isRunning", true);
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
    private IEnumerator ApplyGravity(float fullJumpTime)
    {
        float timeElapsed = 0;
        while (timeElapsed < fullJumpTime)
        {
            gravityMultiplier = gravityValueCurve.Evaluate(timeElapsed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }


    public void JumpAnimationEnded()
    {
        isJumping = false;
        animator.SetBool("isJumping", isJumping);
    }

    public void FallingAnimationEnded()
    {
        isFalling = false;
        animator.SetBool("isFalling", isFalling);
    }
    #endregion

    #region Dashing
    private IEnumerator Dash(float fullDashTime)
    {
        float timeElapsed = 0;
        Vector3 dir = transform.rotation * Vector3.forward;
        while (timeElapsed <= fullDashTime)
        {
            force = dashValueCurve.Evaluate(timeElapsed);
            playerBody.AddForce(dir * force * Time.deltaTime, ForceMode.Impulse);
            playerBody.AddForce(playerBody.velocity * -10f * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Dash Eneded");
        force = 0;
        isDashing = false;
        stopMovementEvent = false;
    }

    public void DashAnimationEnded()
    {
        animator.SetBool("isDashing", false);
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




