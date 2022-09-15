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

    [Header("Distance")]
    [SerializeField] private float defeaultDistance;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    [Header("Smooth/Sharp")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float rotationSharpness;

    //privates

    private Vector3 plannerDirection;  // Camera's postion on the x & z plane
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float targetVerticalAngle;
    private float targetDistance;

    private Vector3 newPosition;
    private Quaternion newRotation;

    //refrencing the private variable in this Monobehavior
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

        // handeling Inputs from mouse to game x,y,and scroll
        float mouseX = PlayerInputs.mouseX;
        float mouseY = PlayerInputs.mouseY;
        float zoom = -PlayerInputs.mouseScroll * zoomSpeed;


        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + mouseY, minVerticalAngle, maxVerticalAngle);
        targetDistance = Mathf.Clamp(targetDistance + zoom, minDistance, maxDistance);

        //Smoothing
                                                                               //Here it is frame dependent
        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness * Time.deltaTime);
        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);




        // End target
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle,0,0);
        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;

        cam.transform.rotation = newRotation;
        cam.transform.position = newPosition;

    }

}
