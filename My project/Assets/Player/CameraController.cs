using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    // members
    [Header("Refrenceing")]
    [SerializeField] private Camera cam = null;
    [SerializeField] private Transform followObj = null;
    private Vector3 camerasTransformPosition;

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

    //For later 
    public float newDistanceFromPlayer; // exploring 
    public float newDistanceFromPlayerInCombat; // combat

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
   
    [Header("Camera Obstruction")]    
    public LayerMask obstructionMasks; // This layermask will be inverted to choose what layers to ignore. 
    Transform Obstruction;


    [Header("Camera Collision")]
    public LayerMask collisionMask;
    public bool isCollisionDetected = false;
    public float cameraSphereRadius = 0.3f;
    public float cameraCollisionOffset = 0.3f;
    public float minimumCollisionOffset = 0.3f;

    [Header("Weapon Wheel Ui")]
    public Canvas seedWheel; //get game object 
    public LayerMask playerObstructsUi; // have it check for player layer 
    public Transform behindPlayer; // starting point 
    public Transform inFrontOfPlayer; // lerp point

     public Vector3 CameraPlannerDirection { get => plannerDirection; }

    #region Unity Functions

    private void Start()
    {
        Obstruction = followObj.transform; // default starting point 
        plannerDirection = followObj.forward;  //Important
        Cursor.lockState = CursorLockMode.Locked;
        if (Cursor.lockState != CursorLockMode.Locked)  // locking the camera if the cursor isn't moving
            return;
    }

    private void Update()
    {     
        if (camPriority == 0)  ExploringCam();
        if (camPriority == 1) CombatCam();

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

        OpenSeedWheel();
    }
    private void FixedUpdate()
    {
        if (CheckForCameraCollisions())
            HandleCameraCollision();
    }

    #endregion

    #region Public Functions


    public void ExploringCam() 
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        if (isCollisionDetected == true && camPriority != 1)
        {

            targetDistance = Mathf.Clamp(newDistanceFromPlayer, 4, defeaultDistance);
            
        }
        else targetDistance = defeaultDistance;


        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + mouseY, minVerticalAngle, maxVerticalAngle);

        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
   
        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        cam.transform.position = newPosition;

        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness * Time.deltaTime);
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        cam.transform.rotation = newRotation;

    }
    public void CombatCam() // Combat camera collision should no be going down the same rate at the exploring cam Fix later
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + mouseY, combatMinVerticalAngle, combatMaxVerticalAngle);

        if (isCollisionDetected == true && camPriority != 0)
        {
            if (newDistanceFromPlayer > 4)
                targetDistance = newDistanceFromPlayer;
            else
                targetDistance = Mathf.Clamp(newDistanceFromPlayer, 7.5f, combatCamDistance);
           
        }
        else targetDistance = combatCamDistance;

        newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
        targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
        cam.transform.position = newPosition;

        newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness * Time.deltaTime);
        targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        cam.transform.rotation = newRotation;
    }
  #endregion

    // Camera Collsion and Obstructions
    private void BlockingSightofPlayer(RaycastHit hit)
    {
        Obstruction = hit.transform;

        if (Obstruction.gameObject.tag == "Enviorment")
        {
            // your being hit run function
            Obstruction.GetComponent<ObstructionView>().SendMessage("Obstructing");
        }
    }

    private bool CheckForCameraCollisions() // becasue this is a fixed upda
    {
        Collider[] colliders = Physics.OverlapSphere(cam.transform.position, cameraSphereRadius, collisionMask, QueryTriggerInteraction.Ignore);
        foreach (var walls in colliders)
        {
            // find the current distance from the player and return true 
            float distanceToPlayer = Vector3.Distance(followObj.position, cam.transform.position);
          
            newDistanceFromPlayer = distanceToPlayer;
            return true;
        }
        // no collision return false
        return false;
    }

    private void HandleCameraCollision()
    {
        isCollisionDetected = true;
        StartCoroutine(PauseCameraForMoment());
        newDistanceFromPlayer = newDistanceFromPlayer / 2f;
    }

    private IEnumerator PauseCameraForMoment()
    {
        yield return new WaitForSeconds(2);
        isCollisionDetected = false;

        // possible error here 
        if (camPriority == 0)
            newDistanceFromPlayer = defeaultDistance;
        else
            newDistanceFromPlayer = combatCamDistance;
    }

    private void OpenSeedWheel()
    {
        if (WeaponWheelController.weaponWheelSelected == true)
        {
            seedWheel.transform.LookAt(Camera.main.transform);
            seedWheel.transform.Rotate(0, 180, 0);

            Vector3 wheel = seedWheel.transform.position;

            Cursor.lockState = CursorLockMode.None;
            var ray = new Ray(wheel, cam.transform.position - seedWheel.transform.position);
            RaycastHit hit;


            // FOr later might have to corutine this or something else 
            Debug.DrawRay(wheel, cam.transform.position - seedWheel.transform.position,Color.blue);
           if (Physics.Raycast(ray, out hit, 50f, playerObstructsUi, QueryTriggerInteraction.Ignore))
           {
                wheel = Vector3.MoveTowards(wheel, inFrontOfPlayer.position, .1f);
                seedWheel.transform.position = wheel;
                Debug.Log("yes");
           }
           else
           {
               wheel = Vector3.MoveTowards(wheel, behindPlayer.position, .1f);
                seedWheel.transform.position = wheel;
               Debug.Log("no");
           }

        }
        else
            Cursor.lockState = CursorLockMode.Locked;

    }




    #region Editor Gizmos 

    private void OnDrawGizmos()
    {
        Handles.DrawLine(cam.transform.position, followObj.position);
        Gizmos.DrawSphere(cam.transform.position, cameraSphereRadius);

        Handles.DrawLine(seedWheel.transform.position, cam.transform.position);
    }



    #endregion
}
