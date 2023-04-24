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
    public SphereCollider collider;
    [SerializeField] private Rigidbody playerBody;

    public LayerMask CollidableLayers;

    private Vector3 movementVector;
    public float force;
    public Keyframe[] keys;
    public AnimationCurve curveToEvaluate;
    public Keyframe lastKey;
    public  float end = 0;

    public AnimationCurve groundedDashValueCurve;
    public AnimationCurve inAirDashValueCurve;


    [Header("Targeting")]
    public bool targetingEnemy;
    public Transform target;
    public float range;

    [Header("Seeds")]
    public int seedId;

    private float gravity;
    public AnimationCurve gravityValueCurve;
    public float gravityMultiplier = 0;
    private IEnumerator gravityCorutine;



    private void Awake()
    {
        cam = GetComponent<CameraController>();
        playerInput = GetComponent<PlayerInput>();
        playerBody = GetComponentInChildren<Rigidbody>();
        playerManger = GetComponent<PlayerManger>();
        animator = GetComponent<PlayerManger>().animator;
        collider = GetComponent<SphereCollider>();

     /*   Keyframe[] keys = gravityValueCurve.keys;
        Keyframe lastKey = keys[keys.Length - 1];
        float end = lastKey.time;
        jumpTime = end;*/

        //We want to make sure when the game starters the animator recognizes the player is on the groun
    //    animator.SetBool("isGrounded", IsGrounded());
    }


    private void Update()
    {
        //Enemy Targetting 
        if (playerInput.target)
            UpdateTarget();
        if (targetingEnemy)
            this.gameObject.transform.LookAt(target);
    }

    private void FixedUpdate()
    {
        

        if (playerManger.subStates == SubStates.Moving)
        {
            CanMove();
        }
    }
    #region Moving
    public void CanMove()
    {
        // if we have input that is either vertical or horizontal then is moving is true 
       playerManger.MovementVector = playerInput.movementInput;
        Vector3 cameraPlannerDirection = cam.CameraPlannerDirection;
        Quaternion cameraPlannerRotation = Quaternion.LookRotation(cameraPlannerDirection);
        //Aligning movement in relation to the camera
        playerManger.MovementVector = cameraPlannerRotation * playerManger.MovementVector;
        
        // checking for inputs from Player
        float speed = playerManger.MovementVector != Vector3.zero ? playerManger.TargetSpeed : 0;

        // Cast a ray in front of the player to check for collisions
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerManger.MovementVector, out hit, speed * Time.fixedDeltaTime, CollidableLayers))
        {
            // If the ray hits something, move the player up to the point of collision
            playerBody.MovePosition(hit.point);
            Debug.Log("Hit Something");
        }
        else
        {
            // If the ray doesn't hit anything, move the player normally
            playerBody.MovePosition(transform.position + playerManger.MovementVector * Time.fixedDeltaTime * speed);
        }

        if (speed != 0)
        {
            playerManger.TargetRot = Quaternion.LookRotation(playerManger.MovementVector);
            transform.rotation = playerManger.TargetRot;
        }
    }
    #endregion


    #region Rotate


    #endregion


    #region Apply Gravity 
    public IEnumerator ApplyGravity()
    {
        float timeElapsed = 0;
        while (playerManger.IsGrounded() == false && playerManger.subStates == SubStates.Attacking)
        {
            gravityMultiplier = gravityValueCurve.Evaluate(timeElapsed);

            playerBody.AddForce(Vector3.down * Time.deltaTime * gravityMultiplier, ForceMode.VelocityChange);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        gravityMultiplier = 0;
    }
    #endregion

    #region Jump
    public void InitateJump()
    {
        if(playerManger.IsGrounded())
        {
            playerBody.AddForce(Vector3.up * playerManger.JumpForce, ForceMode.Impulse);
        }
      
    }

    //Calling an animation envent to stop anim form looping 
 
    #endregion

    #region Dashing'
    public void CreateDash(SuperStates currentState)
    {
        if (currentState== SuperStates.Grounded)
        {
            Debug.Log("ground Dash");
            keys = groundedDashValueCurve.keys;
            curveToEvaluate = groundedDashValueCurve;
            lastKey = keys[keys.Length - 1];
            end = lastKey.time;
        }
        else if (currentState == SuperStates.Falling || playerManger.superStates == SuperStates.Rising)
        {
            Debug.Log("air Dash");
            keys = inAirDashValueCurve.keys;
            curveToEvaluate = inAirDashValueCurve;
            lastKey = keys[keys.Length - 1];
            end = lastKey.time;
        }
        StartCoroutine(Dash());
    }
    public IEnumerator Dash()
    {
       float timeElapsed = 0;
       // bool hasinitalDir = false;
       while (timeElapsed < end)
       {
            Vector3 dir = transform.rotation * Vector3.forward;
          /*if (playerManger.DirectionInput == Vector3.zero)
            {
                
            }
            else
            {
                dir = playerManger.DirectionInput.normalized;
            }*/
            force = curveToEvaluate.Evaluate(timeElapsed);
            playerBody.AddForce(dir * force * Time.deltaTime, ForceMode.VelocityChange);
            playerBody.AddForce(playerBody.velocity * -10f * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
       }
        force = 0;
        end = 0;
        curveToEvaluate = null;

        if (playerManger.superStates == SuperStates.Grounded)
        {
           
            // Stop applying force to the rigidbody
            playerBody.velocity = Vector3.zero;
            playerManger.IsDashing = false;
            playerManger.StopMovement = false;
        }         
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




