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
    [SerializeField] private float zoomSpeed;
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
    private Vector3 worldPosition;
        

    [Header("Combat Cam")]
    [SerializeField] private float newMaxDistance;
    [SerializeField] private float newVerticalAngle;



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

       


        if (camPriority == 0)
        {
            MovementCam();
        }

        else if (camPriority == 1)
        {
         CombatCam();
          
            
           // targetDistance = defeaultDistance;
        }
        else
        {
            camPriority = 0;
        }


       
       
    }


    public void MovementCam()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");
        float zoom = Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;

        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + mouseY, minVerticalAngle, maxVerticalAngle);
        targetDistance = Mathf.Clamp(targetDistance + zoom, minDistance, maxDistance);



        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        cam.transform.position = newPosition;


        
        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness * Time.deltaTime);
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        cam.transform.rotation = newRotation;

        // End target

       
      
    }
    
    //
    public void CombatCam()
    {

        float mouseX = Input.GetAxisRaw("Mouse X");
      
        // we need to change the camera distance from the player and fov
        targetDistance = Mathf.Clamp(newMaxDistance, newMaxDistance, newMaxDistance);
        //this controls horizontal movement 
        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, newVerticalAngle, newVerticalAngle);
        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness * Time.deltaTime);
        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
        // End target
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        cam.transform.rotation = newRotation;
        cam.transform.position = newPosition;
    }

    



}
