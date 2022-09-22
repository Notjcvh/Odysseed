using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References for used Components")] // reference the animator here
    private PlayerInputs inputs;
    private CameraController cam;

    [Header("Movement")]

    [SerializeField] private float runSpeed;
    private float targetSpeed;
   
    private Vector3 newVelocity;
    private Quaternion targetRotation;

    [SerializeField] private float jumpHieght;
    private float jumpPressesd;
    Vector3 newJump;
    // we want to make a bool to control how many times the player can jump



    private void Start()
    {
        inputs = GetComponent<PlayerInputs>();
        cam = GetComponent<CameraController>(); 
    }

    private void Update()
    {
        

        float jump = PlayerInputs.jump * jumpHieght;
        Vector3 movementVector = new Vector3(inputs.MoveAxisRight, 0 , inputs.MoveAxisForward).normalized;
        Vector3 jumpVecotr = new Vector3(0, jump, 0).normalized;
        Vector3 cameraPlannerDirection = cam.CameraPlannerDirection;
        Quaternion cameraPlannerRotation = Quaternion.LookRotation(cameraPlannerDirection);


        Debug.DrawLine(transform.position, transform.position + movementVector, Color.green);



        //Aligning movemnt in relation to the camera
        movementVector = cameraPlannerRotation * movementVector;
        Debug.DrawLine(transform.position, transform.position + movementVector, Color.red);

       // checking for inputs from Player

        targetSpeed = movementVector != Vector3.zero ? runSpeed : 0;
        jumpPressesd = jumpVecotr != Vector3.zero ? jumpHieght : 0;
 

        newVelocity = movementVector * targetSpeed;
        newJump = jumpVecotr * jumpHieght;
        transform.Translate(newVelocity * Time.deltaTime, Space.World);
        transform.Translate(newJump * Time.deltaTime, Space.World);

        Debug.Log(newJump);

        if (targetSpeed != 0)
        {
            targetRotation = Quaternion.LookRotation(movementVector);
            transform.rotation = targetRotation;
        }  
    }


}
