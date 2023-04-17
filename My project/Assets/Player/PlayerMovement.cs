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
    private CapsuleCollider collider;
    public AudioController audioController;
    [SerializeField] private Rigidbody playerBody;

 
    [Header("Movement")]
    public Vector3 movementVector;
    private Quaternion targetRotation;
    public float targetSpeed;
    public bool stopMovementEvent;

    [Header("Jumping")]
    private float gravity;
    public AnimationCurve gravityValueCurve;
    public float gravityMultiplier = 0;
    private IEnumerator gravityCorutine;
    public LayerMask Ground;
    public float lastPlayerPosY;
    public float jumpTime;
    public float jumpForce;
    public float jumpElapsedTime;

    public float distanceToGround;

    [Header("Dash")]
    public AnimationCurve groundedDashValueCurve;
    public AnimationCurve inAirDashValueCurve;
    public float force;
    private IEnumerator dashCorutine;
    public bool isDashing = false;

    [Header("Targeting")]
    public bool targetingEnemy;
    public Transform target;
    public float range;

    [Header("Seeds")]
    public int seedId;

    public bool isGrounded;
    public bool isFalling = false;

    private void Awake()
    {
        cam = GetComponent<CameraController>();
        playerInput = GetComponent<PlayerInput>();
        playerBody = GetComponentInChildren<Rigidbody>();
        playerManger = GetComponent<PlayerManger>();
        animator = GetComponent<PlayerManger>().animator;
        collider = GetComponent<CapsuleCollider>();

        Keyframe[] keys = gravityValueCurve.keys;
        Keyframe lastKey = keys[keys.Length - 1];
        float end = lastKey.time;
        jumpTime = end;

        //We want to make sure when the game starters the animator recognizes the player is on the groun
        animator.SetBool("isGrounded", IsGrounded());
    }


    private void Update()
    {
        
        //Ground Checking
        if (IsGrounded() == true)
        {
            isGrounded = true;
            animator.SetBool("isGrounded", IsGrounded());
            jumpElapsedTime = 0;
            gravityMultiplier = 0;

            if (isFalling == true)
            {
                playerManger.SetPlayerState(PlayerStates.Landing);
                isFalling = false;
            }
        }
        else
        {
            isGrounded = false;
            animator.SetBool("isGrounded", IsGrounded());
        //    gravityCorutine = ApplyGravity();
            
            
            //StartCoroutine(gravityCorutine);
        }

        //Enemy Targetting 
        if (playerInput.target)
            UpdateTarget();
        if (targetingEnemy)
            this.gameObject.transform.LookAt(target);
    }

    private void FixedUpdate()
    {
        if (playerManger.currentState == PlayerStates.Moving || playerManger.currentState == PlayerStates.JumpingAndMoving || playerManger.currentState == PlayerStates.FallingAndMoving )
        {
            if(playerManger.isAttackAnimationActive == false)
                 CanMove();
        }
    }



    #region Moving
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
        if (speed != 0 && playerManger.isDashing == false)
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
        float distanceCheck = collider.bounds.extents.y + distanceToGround;
        Debug.DrawRay(collider.bounds.center, Vector3.down * distanceCheck);
        if (Physics.Raycast(collider.bounds.center, direction, out hit, distanceCheck, Ground))
            return true;
        else
            return false;
    }

    //Checking the change in the position of the player 
    public PlayerStates checkYVelocity(PlayerStates currentState)
    {
        
        if (transform.hasChanged && IsGrounded() == false && playerInput.movementInput == Vector3.zero)
        {
            if (transform.position.y > lastPlayerPosY)
            {
                currentState = PlayerStates.Jumping;
            }
            else if (transform.position.y < lastPlayerPosY)
            {
                currentState = PlayerStates.Falling;
                isFalling = true;
            }
        }
        else if (transform.hasChanged && IsGrounded() == false && playerInput.movementInput != Vector3.zero)
        {
            if (transform.position.y > lastPlayerPosY)
            {
                currentState = PlayerStates.JumpingAndMoving;
            }
            else if (transform.position.y < lastPlayerPosY)
            {
                currentState = PlayerStates.FallingAndMoving;
                isFalling = true;
            }
        }
        lastPlayerPosY = transform.position.y;

        return currentState;
    }


    //Calling an animation envent to stop anim form looping 
    public void FallingAnimationEnded()
    {
        animator.SetBool("isFalling", false);
    }
    #endregion

    #region Apply Gravity 
    private IEnumerator ApplyGravity()
    {
        float timeElapsed = 0;
        while (IsGrounded() == false)
        {
            gravityMultiplier = gravityValueCurve.Evaluate(timeElapsed);

            playerBody.AddForce(Vector3.down * Time.deltaTime * -gravityMultiplier, ForceMode.Acceleration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #region Jump
    public void InitateJump()
    {
        playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    //Calling an animation envent to stop anim form looping 
    public void JumpAnimationEnded()
    {
        animator.SetBool("isJumping", false);
    }
    #endregion

    #region Dashing
    public IEnumerator Dash()
    {

        Keyframe[] keys;
        PlayerStates dashType;
        AnimationCurve curveToEvaluate;
        if (IsGrounded() == true)
        {
            dashType = PlayerStates.GroundedDash;
            keys = groundedDashValueCurve.keys;
            curveToEvaluate = groundedDashValueCurve;
            print("A");
        }
        else
        {
            dashType = PlayerStates.InAirDash;
            keys = inAirDashValueCurve.keys;
            curveToEvaluate = inAirDashValueCurve;
            print("B");
        }
        Keyframe lastKey = keys[keys.Length - 1];
        float end = lastKey.time;
        //Setting to the end of the animation 

        float timeElapsed = 0;
     
        while (timeElapsed < end)
        {
            Vector3 dir = transform.rotation * Vector3.forward;
            force = curveToEvaluate.Evaluate(timeElapsed);
            playerBody.AddForce(dir * force * Time.deltaTime, ForceMode.VelocityChange);
            playerBody.AddForce(playerBody.velocity * -10f * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        animator.SetBool("isDashing", false);
        if (dashType == PlayerStates.GroundedDash)
            playerBody.velocity = Vector3.zero; // Stop player speed instantly 
        else
            playerManger.SetPlayerState(PlayerStates.FallingAndMoving);
    
        
        force = 0;

    }

    public void DashAnimationEnded()
    {
       // animator.SetBool("isDashing", false);
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




    #region Sound looping
    public void SelectAudio(int type)
    {
        if (type == 0)
        {
            AudioType sendingAudio = AudioType.None;
            int numberOfRandomNumbers = 5; // Number of random numbers to generate
            int minRange = 1; // Minimum value for random numbers
            int maxRange = 5;
            for (int i = 0; i < numberOfRandomNumbers; i++)
            {
                int randomNumber = Random.Range(minRange, maxRange + 1); // Generate a random number within the specified range
                Debug.Log("Random Number " + (i + 1) + ": " + randomNumber); // Print the generated random number

                switch (randomNumber)
                {
                    case (1):
                        sendingAudio = AudioType.PlayerWalk1;
                        break;
                    case (2):
                        sendingAudio = AudioType.PlayerWalk2;
                        break;
                    case (3):
                        sendingAudio = AudioType.PlayerWalk3;
                        break;
                    case (4):
                        sendingAudio = AudioType.PlayerWalk4;
                        break;
                    case (5):
                        sendingAudio = AudioType.PlayerWalk5;
                        break;
                  default:
                        break;
                }
            }
            playerManger.ManageAudio(sendingAudio);
        }
        else
        {
            return;
        }
    }

    #endregion


}




