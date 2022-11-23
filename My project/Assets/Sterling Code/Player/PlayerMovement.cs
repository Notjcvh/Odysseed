using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")] 
    private CameraController cam;
    private PlayerInput playerInput;
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Transform camTarget;
    [SerializeField] private Transform distanceToGround;

    public VectorValue startingPosition;

    [Header("Movement")]
    private Vector3 newVelocity;
    private Vector3 movementVector;
    private Quaternion targetRotation;

    private float targetSpeed;

    public float runSpeed;
    public bool stopMovementEvent = false;
    public bool isTalking = false;
    public bool isRestricted = false;
  
    [Header("Jumping")]
    public LayerMask Ground;
    public float verticalVelocity;
    public float distanceToCheckForGround;

    private void Start()
    {
        //for loading up the scene
        transform.position = startingPosition.initialStartValue;
    }
    private void Awake()
    {
        cam = GetComponent<CameraController>();
        playerInput = GetComponent<PlayerInput>();
        playerBody = GetComponentInChildren<Rigidbody>(); 
    }

    private void Update()
    {
        /*  if(isTalking)
         {
             movementPriority = 1;
         }

        if (!isTalking)
         {
             movementPriority = 0;
         }*/

        if (stopMovementEvent == false)
        {
            MoveNow();
            if (IsGrounded() && Jump())
                playerBody.velocity = Vector3.up * verticalVelocity ;                  
        }
        else if(stopMovementEvent == true)
        {
            StopMoving();
        }

    }
    private  void MoveNow()
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
        if (targetSpeed != 0)
        {
            targetRotation = Quaternion.LookRotation(movementVector);
            transform.rotation = targetRotation;
        }
    }
    private bool IsGrounded()
    {
        Vector3 direction = new Vector3(0, -camTarget.position.y , 0);
        RaycastHit hit;
        return Physics.Raycast(camTarget.position, direction.normalized, out hit,distanceToCheckForGround, Ground);      
    }
    private bool Jump()
    {
        return playerInput.jumpInput;
    }
    private void StopMoving()
    {    }
}
