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
     public float copiedDefaultDistance;
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
        
    }


    private void FixedUpdate()
    {
        HandleCameraCollisions(cam.transform.position, copiedDefaultDistance);
    }

    #endregion

    IEnumerator PauseCameraForMoment()
    {
        yield return new WaitForSeconds(7);

        isCollisionDetected = false;
    }



    #region Public Functions
    private void HandleCameraCollisions(Vector3 currentCamPosition, float camDist)
    {
        float position = (currentCamPosition - followObj.position).magnitude; // ray sphere will go along
        RaycastHit hit;

        if (Physics.SphereCast(currentCamPosition, cameraSphereRadius, followObj.position, out hit, Mathf.Abs(position), collisionMask, QueryTriggerInteraction.Ignore))
        {
            copiedDefaultDistance = defeaultDistance;
            float dist = Vector3.Distance(cam.transform.position, hit.point); // get the distance from the camera and the point of contact
            position = -(dist - cameraCollisionOffset);
        }

        if (Mathf.Abs(position) < minimumCollisionOffset)
        {
            print(Mathf.Abs(position));
            isCollisionDetected = true;
            copiedDefaultDistance = Mathf.Lerp(defeaultDistance, position, cameraCollisionOffset);
            StartCoroutine(PauseCameraForMoment());
        }

    }

    public void ExploringCam() 
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        if (isCollisionDetected == true)
        {
            targetDistance = copiedDefaultDistance;
        }
        else targetDistance = defeaultDistance;


        plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + mouseY, minVerticalAngle, maxVerticalAngle);

        
      
        //targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

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
    }

    

    void BlockingSightofPlayer(RaycastHit hit)
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

    private void OnDrawGizmos()
    {
        Handles.DrawLine(cam.transform.position, followObj.position);

        Gizmos.DrawSphere(cam.transform.position, cameraSphereRadius);
    }



    #endregion
}
