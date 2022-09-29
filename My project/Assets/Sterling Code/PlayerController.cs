using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")] 
    private CameraController cam;
    [SerializeField] private Rigidbody playerBody;


    [Header("Movement")]
    [SerializeField] private float runSpeed;
    private float targetSpeed;
    private Vector3 newVelocity;
    private Quaternion targetRotation;
    [Range(1, -1)] private float hor;
    [Range(1, -1)] private float vert;
    public bool isMoving = false;
    public bool isTalking = false;
    public bool isRestricted = false;
    public LayerMask Player;


    [Header("Jumping")]

    public LayerMask Ground;
    public LayerMask Wall;
    [SerializeField] float verticalVelocity = 10;
    [SerializeField] float rayLength;

    // [Header("Ledge Grabbing")]
  //  public Vector3 MovementVector { get => movementVector; }

    private void Start()
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

        
         int movingHorizontal = hor != 0 ? 1 : 0;
         int movingVertical = vert != 0 ? 1 : 0;

        if (movingHorizontal > 0 || movingVertical > 0)
        {
            if(isRestricted == false) MoveNow();
        }
        else
            return;

        if( IsGrounded() && Input.GetButtonDown("Jump"))
        {
           
                playerBody.AddForce(Vector3.up * verticalVelocity, ForceMode.Impulse);
                            
        }

    }
    public void MoveNow()
    {
        // if we have input that is either vertical or horizontal then is moving is true, stop roating toawrds camera 

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
        return Physics.Raycast(transform.position, Vector3.down, rayLength, Ground);
    }

  

}
