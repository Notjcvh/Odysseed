using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;


public class CameraController : MonoBehaviour
{
    // members
    [Header("Refrenceing")]
    [SerializeField] private Camera cam = null;
    public Transform followObj = null;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private CamCollisionDetection camCollision;

    [Header("Mouse Sensitivity")]
    [Range(20, 90)] public float mouseSensitivityX;
    [Range(20, 90)] public float mouseSensitivityY;

    [Header("Vertical Rotation Range")]
    [Range(-90, 90)] [SerializeField] public float minVerticalAngle = -90;  //turning and moving the camera up while in exploring mode 
    [Range(-90, 90)] [SerializeField] public float maxVerticalAngle = 90;
    public float maxVerticalAngleRef { get; private set; }
    public float minVerticalAngleRef { get; private set; }

    [Header("Distance from Target")]
    [Range(4, 10)] [SerializeField] public float defeaultDistance;
    public float DefaultDistanceMax { get; private set; }
    public float DefaultDistanceMin { get; private set; }

    [Header("Rotation Speed")]
    [SerializeField] private float rotationSharpness;

    [Header("Cam Movement and Rotation")]
    private Vector3 plannerDirection;  // Camera's postion on the x & z plane
    private Vector3 targetPosition;   // player's target position
    private Quaternion targetRotation; // player's target rotation
    private Vector3 newPosition;
    private Quaternion newRotation;
    private float targetVerticalAngle;
    private float targetDistance;

   
    [Header("Camera Obstruction")]    
    public LayerMask obstructionMasks; // This layermask will be inverted to choose what layers to ignore. 
    Transform Obstruction;

     public Vector3 CameraPlannerDirection { get => plannerDirection; }

    #region Unity Functions

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        cam = Camera.main;
        Obstruction = followObj.transform; // default starting point 
        plannerDirection = followObj.forward;  //Important

        //Setting properties
        var range = typeof(CameraController).GetField(nameof(maxVerticalAngle)).GetCustomAttribute<RangeAttribute>();
        maxVerticalAngleRef = range.max;
        minVerticalAngleRef = range.min;
     

        var distRange = typeof(CameraController).GetField(nameof(defeaultDistance)).GetCustomAttribute<RangeAttribute>();
        DefaultDistanceMax = distRange.max;
        DefaultDistanceMin = distRange.min;


        // Mouse Lock State
        Cursor.lockState = CursorLockMode.Locked;
        if (Cursor.lockState != CursorLockMode.Locked)  // locking the camera if the cursor isn't moving
            return;
    }


    private void Update()
    {     
         ExploringCam(playerInput.mouseX, playerInput.mouseY);

        var ray = new Ray(cam.transform.position, followObj.position - cam.transform.position);
        RaycastHit hit;
       
        if (Physics.Raycast(ray, out hit, 40, ~(obstructionMasks),QueryTriggerInteraction.Ignore)) 
        {
            
            if (hit.collider.gameObject.tag != "Player")
            {
                BlockingSightofPlayer(hit);
            } 
            else
            {
                if (Obstruction.gameObject.tag == "Enviorment")
                {
                    Obstruction.GetComponent<ObstructionView>().SendMessage("NotObstructing");
                }
            }       
        }
    }
    #endregion

    #region Camera States
    public void ExploringCam(float mouseX,float mouseY)
    {
        targetDistance = defeaultDistance;
        //Mouse Y
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + ((-mouseY) * mouseSensitivityY * Time.deltaTime), minVerticalAngle, maxVerticalAngle);

        //move point to position:
        plannerDirection = Quaternion.Euler(0, mouseX * mouseSensitivityX * Time.deltaTime, 0) * plannerDirection;
   
                         //rotation around y axis                    // rotation around x axis
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness / Time.deltaTime);
        cam.transform.rotation = newRotation;

        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        Vector3 velocity = Vector3.zero;
        newPosition = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref velocity, 0f);
        cam.transform.position = newPosition;
    }
    #endregion


    #region Camera Obstruction
    private void BlockingSightofPlayer(RaycastHit hit)
    {
        Obstruction = hit.transform;

        if (Obstruction.gameObject.tag == "Enviorment")
        {
            // your being hit run function
            Obstruction.GetComponent<ObstructionView>().SendMessage("Obstructing");
        }
    }

    #endregion
}
