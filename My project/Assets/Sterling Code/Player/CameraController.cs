using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Refrenceing")]
    [SerializeField] private Camera cam = null;
    [SerializeField] private Transform followObj = null;
    private PlayerMovement player;

    [Header("XRotations")]
    //turning down and moving the camera up
    [SerializeField] [Range(-90, 90)] private float minVerticalAngle = -90;
    [SerializeField] [Range(-90, 90)] private float maxVerticalAngle = 90;

    [SerializeField] [Range(13, 90)] private float combatMinVerticalAngle = 13;
    [SerializeField] [Range(13, 90)] private float combatMaxVerticalAngle = 90;


    [Header("Priority")]
    public int camPriority = 0;

    [Header("Distance")]
    [SerializeField] private float defeaultDistance;
 
    [Header("Smooth/Sharp")]
   
    [SerializeField] private float rotationSharpness;
    

    [Header("Cam")]
    //privates
    private Vector3 plannerDirection;  // Camera's postion on the x & z plane
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float targetVerticalAngle;
    private float targetDistance;
    private Vector3 newPosition;
    private Quaternion newRotation;


    [SerializeField] private float newMaxDistance;
  
    public Vector3 CameraPlannerDirection { get => plannerDirection; }



    private void Start()
    {
        player = GetComponent<PlayerMovement>();
        //Important
        plannerDirection = followObj.forward;


        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        // locking the camera if the cursor isn't moving
       if (Cursor.lockState != CursorLockMode.Locked)
           return;

        if (camPriority == 0)  MovementCam();
        if (camPriority == 1) CombatCam();

        if (camPriority == 2) LedgeClimbingCam();
    }


    public void MovementCam()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + mouseY, minVerticalAngle, maxVerticalAngle);
        targetDistance = defeaultDistance;
        //targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        cam.transform.position = newPosition;

        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness * Time.deltaTime);
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        cam.transform.rotation = newRotation;
    }
    public void LedgeClimbingCam()
    {
       /* 
        targetDistance = Mathf.Clamp(newMaxDistance, newMaxDistance, newMaxDistance);
      
      //targetHorizontalAngle = Mathf.Clamp(targetHorizontalAngle,minHorizontalAngle, maxHorizontalAngle);

        targetPosition = (followObj.position) - (targetRotation * Vector3.forward) * targetDistance;
        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
        cam.transform.position = newPosition;

        //Find a way for the camera to rotate directyl behind the player! 
        plannerDirection = Quaternion.Euler(0, 90, 0) * plannerDirection;

        // here we're playing with direction so get the test playeer's movement vector 
        //
        Vector3 movementVector = player.MovementVector;

        /* this is basically from the player controller we want to grab the last direction the player was facing 
         * and make the camera face the same way
         * Quaternion cameraPlannerRotation = Quaternion.LookRotation(plannerDirection);
            movementVector = cameraPlannerRotation * movementVector;
            targetRotation = Quaternion.LookRotation(movementVector);
            transform.rotation = targetRotation;
        */
    }

    public void CombatCam()
    {

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + mouseY, combatMinVerticalAngle, combatMaxVerticalAngle);
        targetDistance = defeaultDistance;
        //targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        cam.transform.position = newPosition;

        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness * Time.deltaTime);
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        cam.transform.rotation = newRotation;
    }

}
