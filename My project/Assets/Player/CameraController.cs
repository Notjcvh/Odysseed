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
    [SerializeField] public Transform followObj = null;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;

    [Header("Vertical Rotations")]
    [Range(-90, 90)] public float minVerticalAngle = -90;  //turning and moving the camera up while in exploring mode 
    [Range(-90, 90)] public float maxVerticalAngle = 90;

    
     

    //[SerializeField] [Range(-90, 90)] private float combatMinVerticalAngle = 13; // turning and moving the camera up while in combat mode
    //[SerializeField] [Range(-90, 90)] private float combatMaxVerticalAngle = 90;

    public float maxVerticalAngleRef;
    public float minVertivalAngleRef;





    [Header("Priority")]
    public int camPriority = 0;

    [Header("Distance")]
    [SerializeField] public float defeaultDistance;
    
    [SerializeField] private float combatCamDistance;

    //For later 
    public float newDistanceFromPlayer; // exploring 
    public float newDistanceFromPlayerInCombat; // combat

    [Header("Smooth/Sharp")]
    [SerializeField] private float rotationSharpness;
 // [Range(0, 1)] [SerializeField] private float smoothing = 0.5f;


    [Header("Cam")]
    private Vector3 plannerDirection;  // Camera's postion on the x & z plane
    public Vector3 planDirectionRef;
    private Vector3 targetPosition;   // player's target position
    private Quaternion targetRotation; // player's target rotation

    private Vector3 newPosition;
    private Quaternion newRotation;

    private float targetVerticalAngle;
    private float targetDistance;

    public float testAngle;
   
    [Header("Camera Obstruction")]    
    public LayerMask obstructionMasks; // This layermask will be inverted to choose what layers to ignore. 
    Transform Obstruction;


    [Header("Camera Collision")]
    public LayerMask collisionMask;
    public bool isCollisionDetected = false;
    public float cameraSphereRadius = 0.3f;
    public float cameraCollisionOffset = 0.3f;
    public float minimumCollisionOffset = 0.3f;

   

     public Vector3 CameraPlannerDirection { get => plannerDirection; }

    #region Unity Functions

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        cam = Camera.main;
        Obstruction = followObj.transform; // default starting point 
        plannerDirection = followObj.forward;  //Important
        planDirectionRef = plannerDirection;

        var range = typeof(CameraController).GetField(nameof(maxVerticalAngle)).GetCustomAttribute<RangeAttribute>();
        maxVerticalAngleRef = range.max;
        minVertivalAngleRef = range.min;

        Cursor.lockState = CursorLockMode.Locked;
        if (Cursor.lockState != CursorLockMode.Locked)  // locking the camera if the cursor isn't moving
            return;
    }


    private void Update()
    {     
        if (camPriority == 0)  ExploringCam(playerInput.mouseX, playerInput.mouseY);
       // if (camPriority == 1) CombatCam(playerInput.mouseX, playerInput.mouseY);

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
        if (isCollisionDetected == true && camPriority != 1)
        {
            targetDistance = Mathf.Clamp(newDistanceFromPlayer, 4, defeaultDistance);

        }
        else targetDistance = defeaultDistance;

        //move point to position:
        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
           
        testAngle = targetVerticalAngle;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + (-mouseY), minVertivalAngleRef, maxVerticalAngleRef);

   
                         //rotation around y axis                    // rotation around x axis
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness / Time.deltaTime);
        cam.transform.rotation = newRotation;

        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        Vector3 velocity = Vector3.zero;
        newPosition = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref velocity, 0f);
        cam.transform.position = newPosition;
    }
  /*  public void CombatCam(float mouseX, float mouseY) // Combat camera collision should no be going down the same rate at the exploring cam Fix later
    {
        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + -mouseY, combatMinVerticalAngle, combatMaxVerticalAngle);

        if (isCollisionDetected == true && camPriority != 0)
        {
            if (newDistanceFromPlayer > 4)
                targetDistance = newDistanceFromPlayer;
            else
                targetDistance = Mathf.Clamp(newDistanceFromPlayer, 7.5f, combatCamDistance);
           
        }
        else targetDistance = combatCamDistance;

        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness / Time.deltaTime);
        cam.transform.rotation = newRotation;

        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        Vector3 velocity = Vector3.zero;
        newPosition = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref velocity, 0f);
        // newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
        cam.transform.position = newPosition;
    }*/
    #endregion

    public IEnumerator ReturnCamToDefalut()
    {
        yield return new WaitForSeconds(2);
        minVertivalAngleRef = -30;
        defeaultDistance = 8;
        Debug.Log("Finished");
    }





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

   
    #region Editor Gizmos 
   



    #endregion
}
