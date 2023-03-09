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
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Transform lungePosition;
    [SerializeField] private Transform distanceToGround;
    private Animator animator;

    public AudioController audioController;


    [Header("Movement")]
    private Vector3 newVelocity;
    private Vector3 movementVector;
    private Quaternion targetRotation;

    public float targetSpeed;

    public float runSpeed;
    public bool stopMovementEvent;
    public bool isTalking = false;
    public bool isRestricted = false;

    [Header("Jumping")]
    public LayerMask Ground;
    public float verticalVelocity;
    public float distanceToCheckForGround;

    [Header("Dash")]
    public bool isDashing = false;
    public Transform lerpPosition;
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
        animator = GetComponent<PlayerManger>().animator;
        stopMovementEvent = !stopMovementEvent; //negating the bool value to invert the value of true and false 

       



    }
    private void Update()
    {
      
        if (stopMovementEvent == false)
        {
            MoveNow();
            if (IsGrounded() && Jump())
                playerBody.velocity = Vector3.up * verticalVelocity;
        }
        else if (stopMovementEvent == true)
            StopMoving();

        if (playerInput.dash && dashLifeTimeCounter == 0) 
        {
            dashCoroutine = Dash(transform.position, lerpPosition.position, lerpduration, dashStartValue);
            StartCoroutine(dashCoroutine);
            stopMovementEvent = true;
            isDashing = true;
        }

        if (dashLifeTimeCounter > 0)
            dashLifeTimeCounter -= 1 * Time.deltaTime;
        else
            dashLifeTimeCounter = 0;


        if (isDashing == true)
        {
            RaycastHit hit;
            float range = 2f;
            Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward * range));
            if (Physics.Raycast(ray, range, playerCollionMask, QueryTriggerInteraction.Ignore))
            {
                StopCoroutine(dashCoroutine);
                stopMovementEvent = false;
                isDashing = false;

            }
        }
 
        //Enemy Targetting 
        if (playerInput.target)
            UpdateTarget();
        if (targetingEnemy)
            this.gameObject.transform.LookAt(target);
    }

    #region Player Movement and Stop Movement 
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
        transform.Translate(newVelocity * Time.deltaTime, Space.World);
        if (targetSpeed != 0 && isDashing == false)
        {
            targetRotation = Quaternion.LookRotation(movementVector);
            transform.rotation = targetRotation;
            animator.SetBool("isRunning", true);
        }
        else
            animator.SetBool("isRunning", false);
    }
    private void StopMoving()
    {
        Debug.Log("Movement should be not working");
    }
    #endregion

    #region Ground Check and Jumping
    public bool IsGrounded()
    {
        Vector3 direction = new Vector3(0, -transform.position.y, 0);
        RaycastHit hit;
        return Physics.Raycast(transform.position, direction.normalized, out hit, distanceToCheckForGround, Ground);
    }
    private bool Jump()
    {
        return playerInput.jumpInput;
    }
    #endregion

    #region Dashing
    private IEnumerator Dash(Vector3 currentPostion, Vector3 endPosition, float lerpDuration, float dashtime)
    {
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


    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        Debug.Log("AHHHHHHHHHHH");
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
            targetingEnemy = true;
        }
        else
        {
            target = null;
            targetingEnemy = false;
        }
    }
    

            

private void OnDrawGizmos()
    {
        float range = 2f;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * range));
    }

}
