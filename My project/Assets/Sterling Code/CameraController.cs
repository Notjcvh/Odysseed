using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Refrenceing")]
    [SerializeField] private Camera cam = null;
    [SerializeField] private Transform followObj = null;

    [Header("XRotations")]
    //turning down and moving the camera up
    [SerializeField] [Range(-90, 90)] private float minVerticalAngle = -90;
    [SerializeField] [Range(-90, 90)] private float maxVerticalAngle = 90;

   
    [Header("Priority")]
    public int camPriority = 0;

    [Header("Distance")]
    [SerializeField] private float defeaultDistance;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
 

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
    private float targetHorizontalAngle;
    [SerializeField] [Range(0, 360)] private float minHorizontalAngle;
    [SerializeField] [Range(0, 360)] private float maxHorizontalAngle; 
    public Vector3 CameraPlannerDirection { get => plannerDirection; }

    private void Start()
    {
        //Important
        plannerDirection = followObj.forward;

        targetDistance = defeaultDistance;

        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        // locking the camera if the cursor isn't moving
       if (Cursor.lockState != CursorLockMode.Locked)
           return;

        if (camPriority == 0)  MovementCam();


        if (camPriority == 1) LedgeClimbingCam();
        

    }


    public void MovementCam()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");
        float zoom = Input.GetAxisRaw("Mouse ScrollWheel");

        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + mouseY, minVerticalAngle, maxVerticalAngle);
        targetDistance = Mathf.Clamp(targetDistance + zoom, minDistance, maxDistance);

        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        cam.transform.position = newPosition;
        
        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness * Time.deltaTime);
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        cam.transform.rotation = newRotation;

    }
    public void LedgeClimbingCam()
    {
        targetDistance = Mathf.Clamp(newMaxDistance, newMaxDistance, newMaxDistance);
        plannerDirection = Quaternion.Euler(0, 90, 0) * plannerDirection;
        targetHorizontalAngle = Mathf.Clamp(targetHorizontalAngle,minHorizontalAngle, maxHorizontalAngle);
        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        cam.transform.position = newPosition;

       //Find a way for the camera to rotate directyl behind the player! 
    }
}
