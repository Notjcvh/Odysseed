using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // members
    [Header("Refrenceing")]
    [SerializeField] private Camera cam = null;
    [SerializeField] private Transform followObj = null;

    [Header("Vertical Rotations")]
   
    [SerializeField] [Range(-90, 90)] private float minVerticalAngle = -90;  //turning and moving the camera up while in exploring mode 
    [SerializeField] [Range(-90, 90)] private float maxVerticalAngle = 90;   
    [SerializeField] [Range(13, 90)] private float combatMinVerticalAngle = 13; // turning and moving the camera up while in combat mode
    [SerializeField] [Range(13, 90)] private float combatMaxVerticalAngle = 90;

    [Header("Priority")]
    public int camPriority = 0;

    [Header("Distance")]
    [SerializeField] private float defeaultDistance;
    [SerializeField] private float combatCamDistance;

    [Header("Smooth/Sharp")]
    [SerializeField] private float rotationSharpness;
    [Range(0, 1)] [SerializeField] private float smoothing = 0.5f;


    [Header("Cam")]
    private Vector3 plannerDirection;  // Camera's postion on the x & z plane
    private Vector3 targetPosition;   // player's target position
    private Quaternion targetRotation; // player's target rotation

    private Vector3 newPosition;
    private Quaternion newRotation;

    private float targetVerticalAngle;
    private float targetDistance;
   
    [Header("Camera Clipping")]    
    public GameObject camClippingSphere;
    [SerializeField] private LayerMask walls;
    [SerializeField] private float camRayLength = 5;
    
    public Vector3 CameraPlannerDirection { get => plannerDirection; }

    #region Unity Functions

    private void Start()
    {
        plannerDirection = followObj.forward;  //Important
        Cursor.lockState = CursorLockMode.Locked;
        if (Cursor.lockState != CursorLockMode.Locked)  // locking the camera if the cursor isn't moving
            return;
    }

    private void Update()
    {
        if (camPriority == 0)  ExploringCam();
        if (camPriority == 1) CombatCam();
        ScaleClipSphere();
    }

    #endregion

    #region Public Functions
    public void ExploringCam() 
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + mouseY, minVerticalAngle, maxVerticalAngle);
        targetDistance = defeaultDistance;

        //targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        float rate = Smooth(cam.transform.position.y, targetPosition.y, smoothing, Time.deltaTime);

        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
   
        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        cam.transform.position = newPosition;

        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness * Time.deltaTime);
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        cam.transform.rotation = newRotation;

    }
    public void CombatCam()
    {

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + mouseY, combatMinVerticalAngle, combatMaxVerticalAngle);
        targetDistance = combatCamDistance;
        //targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        cam.transform.position = newPosition;

        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness * Time.deltaTime);
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        cam.transform.rotation = newRotation;

        camRayLength = 9;
    }

    void ScaleClipSphere() // for camera clipping test right now
    {
        RaycastHit hit;
        Vector3 objectScale = camClippingSphere.transform.localScale;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, camRayLength, walls))
        {
            objectScale.Set(6, 6, 6);
            camClippingSphere.transform.localScale = objectScale;
        }
        else
        {
            objectScale.Set(0, 0, 0);
            camClippingSphere.transform.localScale = objectScale;
        }
    }
    #endregion

    #region Unused Functions
    public static float Smooth(float source, float target, float rate, float dt)
    {
        return Mathf.Lerp(source, target, 1 - Mathf.Pow(rate, dt));
    }
    #endregion
}
