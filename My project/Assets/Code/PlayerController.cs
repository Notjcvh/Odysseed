using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References for used Components")] // reference the animator here
 
    private CameraController cam;
    private Rigidbody playerBody;

    [Header("Movement")]

    [SerializeField] private float runSpeed;
    private float targetSpeed;
    private Vector3 newVelocity;
    private Quaternion targetRotation;

    [Header("Jumping")]
    public LayerMask Ground;
    [SerializeField] float verticalVelocity = 10;
    [SerializeField] float rayLength;

    [Header("Player Stats")]
    public int maxHealth = 10;
    int currentHealth;
   



    private void Start()
    {
        cam = GetComponent<CameraController>();
        playerBody = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
   
        Vector3 movementVector = new Vector3(horizontal, 0, vertical).normalized;

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

        if( IsGrounded() && Input.GetButtonDown("Jump"))
        {
            //playerBody.velocity = Vector3.up * verticalVelocity;
            playerBody.AddForce(Vector3.up * verticalVelocity, ForceMode.Impulse);
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            TakeDamage(5);
            Debug.Log(currentHealth);
        }

    }

   private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, rayLength, Ground);
    }

   public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

}
