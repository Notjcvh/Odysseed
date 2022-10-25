using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")] 
    private CameraController cam;
    [SerializeField] private Rigidbody playerBody;
    
    [SerializeField] private Transform camTarget;
    [SerializeField] private Transform distanceToGround;



    public int movementPriority = 0;

    [Header("Movement")]

    private Vector3 movementVector;    
    [SerializeField] private float runSpeed;
    private float targetSpeed;
    private Vector3 newVelocity;
    private Quaternion targetRotation;
    [Range(1, -1)] private float hor;
    [Range(1, -1)] private float vert;
    public bool isMoving = false;
    public bool isTalking = false;
    public bool isRestricted = false;
  

    [Header("Jumping")]

    public LayerMask Ground;
    [SerializeField] float verticalVelocity;
    [SerializeField] float distanceToCheckForGround;

   
    public Vector3 MovementVector { get => movementVector;}

    private void Awake()
    {
        cam = GetComponent<CameraController>();
        playerBody = GetComponentInChildren<Rigidbody>();
    }


    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        hor = horizontal;
        vert = vertical;
        if(isTalking)
        {
            movementPriority = 1;
        }
        if (!isTalking)
        {
            movementPriority = 0;
        }

        int movingHorizontal = hor != 0 ? 1 : 0;
        int movingVertical = vert != 0 ? 1 : 0;

        if (movementPriority == 0 )
        {
            if(IsGrounded() && Input.GetButtonDown("Jump"))
           
                playerBody.AddForce(Vector3.up * verticalVelocity, ForceMode.Impulse);                     
            if (movementPriority == 0) MoveNow();
        }
        else
            return;
    }
    private  void MoveNow()
    {
        // if we have input that is either vertical or horizontal then is moving is true, stop roating 

        Vector3 movementVector = new Vector3(hor, 0, vert).normalized;

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
}
