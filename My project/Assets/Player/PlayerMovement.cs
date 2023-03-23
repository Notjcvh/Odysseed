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
    public Vector3 newVelocity;
    private Vector3 movementVector;
    private Quaternion targetRotation;

    public float targetSpeed;

    public float runSpeed;
    public bool stopMovementEvent;
    public bool isFalling;
    public bool isTalking = false;
    public bool isRestricted = false;

    [Header("Jumping")]
    public LayerMask Ground;

    public float jumpForce = 10f;
   
    public float maxJumpTime = 1f; // the max time for the jump to complete, from standstill to standstill 
    public float gravity = 6; // our accleration 
    public float maxJumpHeight = 1;
    public float currentJumpTime = 0; //the current amount of time passed since the first frame of the jump 
    public float maxJumpTime2 = 2f;
    public float maxJumpHeight2= 1;



    //
    public AnimationCurve jumpCurve;
    public float jumpHieghtMultiplier = 2; // applied to jump force based on how long the button is held down for
  

    // Gravity

   



    public float time;
    private Vector3 fallingVector;
   // public AnimationCurve gravity;
    private float gravitySpeed;

    
    
    // Possibly will be removed 
    [SerializeField] private float verticalVelocity;
    public Vector3 playerVerticalVelocity;


    [Header("Dash")]
    public bool isDashing = false;
    public Transform lerpToPosition;
    public float dashStartValue;

    // 
    public float dashLifeTimeCounter = 0;

    public float lerpduration;
    public LayerMask playerCollionMask;
    IEnumerator dashCoroutine;

    [Header("Targeting")]
    public bool targetingEnemy;
    public Transform target;
    public float range;

    [Header("Seeds")]
    public int seedId;

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


        //Set gravity allready 
        float jumpApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(jumpApex, 2);

    }
    private void FixedUpdate()
    {

        CheckGravity();
        #region Locomotion Inputs
        if (playerInput.movementInput != Vector3.zero && stopMovementEvent != true )
        {
            MoveNow();
            if (playerInput.jumpInput && IsGrounded() || playerInput.jumpInput && IsGrounded() == false && currentJumpTime < maxJumpTime)
            {
                Debug.Log("Jump Pressed");
                CalculateJump(newVelocity);
                currentJumpTime += Time.fixedDeltaTime;
                //jumpHieghtMultiplier += Time.deltaTime;
                playerManger.currentState = PlayerStates.Jumping;
            }

            animator.SetBool("isRunning", true);
            animator.SetBool("isGrounded", IsGrounded());
        
            playerManger.currentState = PlayerStates.Moving;
        }
        else if(stopMovementEvent != true && isFalling != true)
        {
            playerManger.currentState = PlayerStates.Idle;
            newVelocity = Vector3.zero;
            animator.SetBool("isRunning", false);
            if (playerInput.jumpInput && IsGrounded() || playerInput.jumpInput && IsGrounded() == false && currentJumpTime < maxJumpTime)
            {
                Debug.Log("Jump Pressed");
                CalculateJump(newVelocity);
                currentJumpTime += Time.fixedDeltaTime;
                //jumpHieghtMultiplier += Time.deltaTime;
                playerManger.currentState = PlayerStates.Jumping;
            }
        }

        // ground Check 
        if (IsGrounded() == true) // player is on the ground 
        {
            playerBody.drag = 7;
            animator.SetBool("isGrounded", IsGrounded());
   
            if (isFalling == true || currentJumpTime > maxJumpTime)
            {
                jumpHieghtMultiplier = 2;
                currentJumpTime = 0;
                isFalling = false;
                time = 0;
                gravity = 0;

            }
        }
        else if (IsGrounded() == false)  // player is in the air 
        {
            playerBody.drag = 0;
            animator.SetBool("isGrounded", IsGrounded());
            animator.SetBool("isRunning", false);
        }
        bool isGrounded = IsGrounded();
       
       

        //Dashing
        if (dashLifeTimeCounter > 0)
            dashLifeTimeCounter -= 1 * Time.deltaTime;
        else
            dashLifeTimeCounter = 0;

        if (playerInput.dash && dashLifeTimeCounter == 0) 
        {
            dashCoroutine = Dash(transform.position, lerpToPosition.position, lerpduration, dashStartValue);
            StartCoroutine(dashCoroutine);
            stopMovementEvent = true;
            isDashing = true;
            playerManger.currentState = PlayerStates.Dashing;
        }

        if (isDashing == true)
        {
           //aycastHit hit;
            float range = 2f;
            Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward * range));
            if (Physics.Raycast(ray, range, playerCollionMask, QueryTriggerInteraction.Ignore))
            {
                StopCoroutine(dashCoroutine);
                stopMovementEvent = false;
                isDashing = false;
                playerManger.currentState = PlayerStates.Idle;
            }
        }
        #endregion


        //Enemy Targetting 
        if (playerInput.target)
            UpdateTarget();
        if (targetingEnemy)
            this.gameObject.transform.LookAt(target);
    }



    private void CheckGravity()
    {
        // Apply Gravity 
        if (IsGrounded() == false || playerBody.velocity.y != 0 )
        {
            Debug.Log("AppyGravity");
            newVelocity.y += gravity * Time.fixedDeltaTime;
            playerBody.velocity = newVelocity;
        }
    }

    #region Moving the Player
    private void MoveNow()
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
        //   transform.Translate(newVelocity * Time.deltaTime, Space.World);
        playerBody.velocity = newVelocity;
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
        {
            isFalling = true;
            return false;
        }  
    }
    #endregion

    #region Jumping     
    private void CalculateJump(Vector3 velocity)
    {

        //Jump 1
        //caluclate basic gravity based of time duration 

        if (velocity == Vector3.zero)
        {
            float jumpApex = maxJumpTime / 2;
            gravity = (-2 * maxJumpHeight) / Mathf.Pow(jumpApex, 2);
            if (currentJumpTime <= jumpApex)
            {
                Debug.Log("Jump based on time duration: Stationary");
                float intialjumpVelocity = (2 * maxJumpHeight) / jumpApex;
                newVelocity.y = intialjumpVelocity;
                playerBody.velocity = newVelocity;
            }

        }

        //Calculate Jump 2 with respect to horizontal velocity
        else if(velocity != Vector3.zero)
        {

            //add movement vector and vector 3.zero 
            Vector3 c = new Vector3(movementVector.x + 0, movementVector.y + 1, movementVector.z + 0);
            float angle = Vector3.Angle(movementVector, c); // should co

            float intialHor = movementVector.magnitude * Mathf.Cos(angle);
            float intialVert = Vector3.up.magnitude * Mathf.Sin(angle);

            float horDisplacement = intialHor * maxJumpTime2;
            float vertDisplacement = intialVert * maxJumpTime2;

            gravity = (-2 * maxJumpHeight2 * (intialHor * intialHor)) / ((horDisplacement * horDisplacement)/2);

            vertDisplacement = (2 * maxJumpTime2 * intialHor) / (horDisplacement / 2);
            newVelocity.y = vertDisplacement * runSpeed;
            playerBody.velocity = newVelocity;         
            
        }

        
    }
    #endregion

    #region Dashing
    private IEnumerator Dash(Vector3 currentPostion, Vector3 endPosition, float lerpDuration, float dashtime)
    {
        audioController.PlayAudio(AudioType.PlayerAttack, false, 0, false);
        dashLifeTimeCounter = dashtime;
        //this is for sliding 
        for (float t = 0; t < 1; t += Time.deltaTime / lerpduration)
        {
            transform.position = Vector3.Lerp(currentPostion, endPosition, t);
            yield return null;
        }
        isDashing = false;
        stopMovementEvent = false; 
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
        Gizmos.DrawRay(distanceToGround.position, c.normalized * 10);
        if (currentJumpTime <= maxJumpTime)
        {
            //calculate a direction from ground
            Vector3 direction = Vector3.Normalize(playerBody.position - distanceToGround.position);

            float multiplier = jumpCurve.Evaluate(currentJumpTime / (maxJumpTime * jumpHieghtMultiplier));

            Vector3 jumpVector = new Vector3(0, direction.y * multiplier * jumpForce, 0);

            Gizmos.DrawRay(jumpVector, direction);

            //playerBody.AddForce(jumpVector, ForceMode.Impulse);
        }
    }

}
