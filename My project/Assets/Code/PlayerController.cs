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



    private void Start()
    {
        inputs = GetComponent<PlayerInputs>();
        cam = GetComponent<CameraController>(); 
    }

    private void Update()
    {
       
        Vector3 movementVector = new Vector3(inputs.MoveAxisRight, 0, inputs.MoveAxisForward).normalized;
        Vector3 cameraPlannerDirection = cam.CameraPlannerDirection;
        Quaternion cameraPlannerRotation = Quaternion.LookRotation(cameraPlannerDirection);


        Debug.DrawLine(transform.position, transform.position + movementVector, Color.green);

        //Aligning movemnt in relation to the camera
        movementVector = cameraPlannerRotation * movementVector;
        Debug.DrawLine(transform.position, transform.position + movementVector, Color.red);

       // target speed checking for input
        targetSpeed = movementVector != Vector3.zero ? runSpeed : 0;

        newVelocity = movementVector * targetSpeed;
        transform.Translate(newVelocity * Time.deltaTime, Space.World);

        if (targetSpeed != 0)
        {
            targetRotation = Quaternion.LookRotation(movementVector);
            transform.rotation = targetRotation;
        }

    }


}
