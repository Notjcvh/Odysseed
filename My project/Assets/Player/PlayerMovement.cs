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

    [Header("Collision")]
    public float offset = 0.1f;
    public LayerMask CollidableLayers;


    [Header("Dash Force")]
    private Keyframe lastKey;
    private float end = 0;
    private Keyframe[] keys;
    private AnimationCurve curveToEvaluate;
    public float dashForce;
    public AnimationCurve groundedDashValueCurve;
    public AnimationCurve inAirDashValueCurve;

    [Header("Player Applied Gravity")]
    private float gravity;
    public AnimationCurve fallingAttackGravity;
    public AnimationCurve dashingGravityCurve;
    public AnimationCurve jumpGravity;
    public float gravityMultiplier = 0;
    private IEnumerator gravityCorutine;


    //Unused stuff
    //[Header("Targeting")]
    //public bool targetingEnemy;
    //public Transform target;
    //public float range;
    //[Header("Seeds")]
    //public int seedId;

    private void Awake()
    {
        cam = GetComponent<CameraController>();
        playerInput = GetComponent<PlayerInput>();
        playerManger = GetComponent<PlayerManger>();
        animator = GetComponent<PlayerManger>().animator;

     /*   Keyframe[] keys = gravityValueCurve.keys;
        Keyframe lastKey = keys[keys.Length - 1];
        float end = lastKey.time;
        jumpTime = end;*/

        //We want to make sure when the game starters the animator recognizes the player is on the groun
    //    animator.SetBool("isGrounded", IsGrounded());
    }


    private void Update()
    {
        ////Enemy Targetting 
        //if (playerInput.target)
        //    UpdateTarget();
        //if (targetingEnemy)
        //    this.gameObject.transform.LookAt(target);
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
        if (Physics.Raycast(transform.position + (playerManger.MovementVector.normalized * offset), playerManger.MovementVector, out hit, speed * Time.fixedDeltaTime, CollidableLayers))
        {
            // If the ray hits something, move the player up to the point of collision with the offset added
           playerManger.playerBody.MovePosition(hit.point - (playerManger.MovementVector.normalized * offset));
        }
        else
        {
            // If the ray doesn't hit anything, move the player normally
          playerManger.playerBody.MovePosition(transform.position + playerManger.MovementVector * Time.fixedDeltaTime * speed);
        }

        if (speed != 0)
        {
            playerManger.TargetRot = Quaternion.LookRotation(playerManger.MovementVector);
            transform.rotation = playerManger.TargetRot;
        }
    }
    #endregion
    #region Apply Gravity 
    public IEnumerator ApplyGravity()
    {
        float timeElapsed = 0;
        while (playerManger.IsGrounded() == false)
        {
            Debug.Log("Called");
            if (playerManger.subStates != SubStates.Attacking && playerManger.subStates != SubStates.Dashing)
            {
                gravityMultiplier = jumpGravity.Evaluate(timeElapsed);
            }
            else if(playerManger.subStates == SubStates.Dashing)
            {
                gravityMultiplier = dashingGravityCurve.Evaluate(timeElapsed);
            }
            else if(playerManger.subStates == SubStates.Attacking)
            {
               
                gravityMultiplier = fallingAttackGravity.Evaluate(timeElapsed);
            }
            playerManger.playerBody.AddForce(Vector3.down * Time.deltaTime * gravityMultiplier, ForceMode.VelocityChange);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        gravityMultiplier = 0;
    }
    #endregion
    #region Jump
    public void InitateJump()
    {
        playerManger.playerBody.AddForce(Vector3.up * playerManger.JumpForce, ForceMode.Impulse);
    }
    #endregion
    #region Dashing'
    public void CreateDash(SuperStates currentState)
    {
        if (currentState == SuperStates.Grounded)
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
            StopCoroutine(ApplyGravity());
        }
        StartCoroutine(Dash());
    }
    public IEnumerator Dash()
    {
       float timeElapsed = 0;
       while (timeElapsed < end)
       {
            Vector3 dir = transform.rotation * Vector3.forward;
            dashForce = curveToEvaluate.Evaluate(timeElapsed);
            playerManger.playerBody.AddForce(dir * dashForce * Time.deltaTime, ForceMode.VelocityChange);
            playerManger.playerBody.AddForce(playerManger.playerBody.velocity * -10f * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
       }
        dashForce = 0;
        end = 0;
        curveToEvaluate = null;

        if (playerManger.superStates == SuperStates.Grounded)
        {
            // Stop applying force to the rigidbody
            playerManger.playerBody.velocity = Vector3.zero;
            playerManger.IsDashing = false;
            playerManger.StopMovement = false;
        }         
    }

    public void DashAnimationEnded()
    {
       // animator.SetBool("isDashing", false);
    }
    #endregion
    //#region Player Targeting 
    //void UpdateTarget()
    //{
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    float shortestDistance = Mathf.Infinity;
    //    GameObject nearestEnemy = null;
    //    Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
    //    foreach (GameObject enemy in enemies)
    //    {
    //        Vector3 TargetScreenPoint = Camera.main.WorldToScreenPoint(enemy.transform.position);
            
    //        float distance = Vector2.Distance(TargetScreenPoint, screenCenter);

    //        if (distance < shortestDistance)
    //        {
    //            shortestDistance = distance;
    //            nearestEnemy = enemy;
    //        }
    //    }

    //    if (nearestEnemy != null && shortestDistance <= range)
    //    {
    //        target = nearestEnemy.transform;
    //        nearestEnemy.GetComponent<Enemy>().isTargeted = true;
    //    }

    //}
    //#endregion





}




