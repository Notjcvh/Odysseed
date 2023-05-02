using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;



public class CamCollisionDetection : MonoBehaviour
{
    //Reference to Camera Controller 
    public CameraController camControl;

    private int groundLayer = (1 << 8);
    private int wallLayer = (1 << 10);
    private int ceilingLayer = (1 << 14);

    public LayerMask collidableLayers;


    public float contactPoint;
    public float detectionDistance;


    private float defaultMaxVertivalAngle;
    private float defaultMinVerticalAngle;



    public float defaultDistanceMax;
    public float defaultDistanceMin;
    public float newCamDist;
    //Methods

    [Header("Camera Collision")]
    public bool isGroundCollisionDetected = false;
    public bool isWallCollisionDetected = false;
    public bool isCeilingCollisionDetected = false;

    private Coroutine alterCamDistance;
    private Coroutine returnCamDistance;


    private bool isCoroutineRunning = false;

    public bool isTriggerEntered;
    public List<Collider> collider;
    public List<Collider> ground;

    public float offset = 1.5f;


    void OnDrawGizmos()
    {
        //  Vector3 direct = Vector3.Normalize(camControl.followObj.position - transform.position);
        // float distance = Mathf.Ceil((transform.position - camControl.followObj.position).magnitude);
        //print(distance);
        //Gizmos.DrawRay(transform.position, direct);
    }
    private void Start()
    {
        camControl = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraController>();
        defaultDistanceMax = camControl.DefaultDistanceMax;
        defaultDistanceMin = camControl.DefaultDistanceMin;

        defaultMaxVertivalAngle = camControl.maxVerticalAngleRef;
        defaultMinVerticalAngle = camControl.minVerticalAngleRef;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (((1 << collision.gameObject.layer) & collidableLayers) != 0)
        {
            if (!collider.Contains(collision))
            {
                collider.Add(collision);
            }
        }
    }


    private void OnTriggerStay(Collider collision)
    {
        if (((1 << collision.gameObject.layer) & collidableLayers) != 0)
        {
            if (!collider.Contains(collision))
            {
                collider.Add(collision);
            }
        }

    }
    private void Update()
    {
        if(collider?.Count > 0)
        {
            foreach (Collider obj in collider)
            {
                if (groundLayer == (groundLayer & (1 << obj.gameObject.layer)))
                {
                    if(!ground.Contains(obj))
                    {
                        ground.Add(obj);
                    }
                    isGroundCollisionDetected = true;
                    RaycastHit hit;
                    Ray downRay = new Ray(transform.position, Vector3.down);
                    if (Physics.Raycast(downRay, out hit, groundLayer))
                    {
                       foreach (Collider item in ground)
                       {
                            if(hit.transform.gameObject != item.gameObject)
                            {
                                ground.Remove(obj);
                                collider.Remove(obj);
                            }    
                       }
                        #region  Checking For Distance from Ground
                        //A
                        float distanceToGround = (transform.position - hit.point).magnitude;
                        //B
                        Vector3 direct = Vector3.Normalize(transform.position - camControl.followObj.position);
                        float distancePlayerToCam = (transform.position - camControl.followObj.position).magnitude;

                        //C
                        Vector3 directionToGround = Vector3.Normalize(hit.point - camControl.followObj.position);
                        float distancePlayerToGround = (hit.point - camControl.followObj.position).magnitude;

                       
                        //CamController
                        Debug.DrawRay(camControl.followObj.position, direct * distancePlayerToCam, Color.blue);
                        Debug.DrawRay(transform.position, Vector3.down * distanceToGround, Color.green);
                        Debug.DrawRay(camControl.followObj.position, directionToGround * distancePlayerToGround, Color.red);

                        HandleGroundCollision(hit.point.y);

                        #endregion
                    }
                }

                if (ceilingLayer == (ceilingLayer & (1 << obj.gameObject.layer)))
                {
                    RaycastHit hit;
                    Ray upRay = new Ray(transform.position, Vector3.up);
                    if (Physics.Raycast(upRay, out hit, groundLayer))
                    {
                        float distanceToCeiling = (transform.position - hit.point).magnitude;

                        Vector3 direct = Vector3.Normalize(transform.position - camControl.followObj.position);
                        float distancePlayerToCam = (transform.position - camControl.followObj.position).magnitude;

                        //C
                        Vector3 directionToCeiling = Vector3.Normalize(hit.point - camControl.followObj.position);
                        float distancePlayerToGround = (hit.point - camControl.followObj.position).magnitude;

                        Debug.DrawRay(camControl.followObj.position, direct * distancePlayerToCam, Color.blue);
                        Debug.DrawRay(transform.position, Vector3.up * distanceToCeiling, Color.green);
                        Debug.DrawRay(camControl.followObj.position, directionToCeiling * distancePlayerToGround, Color.red);

                        HandleCeilingCollision(hit.point.y);
                    }
                }
                
                if (wallLayer == (wallLayer & (1 << obj.gameObject.layer)))
                {
                    isWallCollisionDetected = true;
                    Vector3[] points = new Vector3[0];

                    Vector3 point = obj.ClosestPoint(this.transform.position); // for some reason this returns two points however we just want the point shooting out horizontally
                    Vector3 closestPointHorizontal = new Vector3(point.x, transform.position.y, point.z); //this gets that for us
                    Vector3 direction = (closestPointHorizontal - this.transform.position);

                    float horizontaldistance = (transform.position - closestPointHorizontal).magnitude;
                    if (horizontaldistance < 1)
                    {
                        newCamDist = camControl.defeaultDistance - horizontaldistance;
                        if(returnCamDistance != null)
                        {
                            StopCoroutine(returnCamDistance);
                            returnCamDistance = null;
                        }
                        
                        if(!isCoroutineRunning)
                        {
                            isCoroutineRunning = true;
                            alterCamDistance = StartCoroutine(AlterCameraDistance(1f));
                           
                        }
                    }

                    Debug.DrawRay(this.transform.position, direction.normalized * horizontaldistance, Color.red);
                }
            }
          
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        // Run the checks 
        if (groundLayer == (groundLayer & (1 << collision.gameObject.layer)))
        {
            camControl.minVerticalAngle = defaultMinVerticalAngle;
        //    print("Default min vertical: " + camControl.minVerticalAngle);
            isGroundCollisionDetected = false;
        }
        collider.Remove(collision);

        if (ceilingLayer == (ceilingLayer & (1 << collision.gameObject.layer)))
        {
            isCeilingCollisionDetected = false;
        }

        if (wallLayer == (wallLayer & (1 << collision.gameObject.layer)))
        {
            isWallCollisionDetected = false;
          
            returnCamDistance =  StartCoroutine(ReturnCamToNormal());
        }
        collider.Remove(collision);
    }
    void HandleGroundCollision(float hitObjectY)
    {    
        Vector3 followObjUp = camControl.followObj.up;
        Vector3 directtoCam = Vector3.Normalize(transform.position - camControl.followObj.position);
        float dot = Vector3.Dot(directtoCam, followObjUp);
        dot = Mathf.Clamp(dot, -1, 1);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float degree = (-camControl.minVerticalAngleRef) - angle + offset;
        float differenceInTheY = this.transform.position.y - hitObjectY;

        if (differenceInTheY < 1)
        {
            camControl.minVerticalAngle = degree ;
            //print("Distance to ground :" + differenceInTheY + "degree : " + degree  + "cam control : " + camControl.minVerticalAngle); 
        }
    }

    void HandleCeilingCollision(float hitObjectY)
    {
        Vector3 followObjUp = camControl.followObj.up;
        Vector3 directtoCam = Vector3.Normalize(transform.position - camControl.followObj.position);
        float dot = Vector3.Dot(directtoCam, -followObjUp);
        dot = Mathf.Clamp(dot, -1, 1);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float degree =  angle - camControl.maxVerticalAngleRef ;
        float differenceInTheY = hitObjectY - transform.position.y;
      
        //more checks 

        if (differenceInTheY <= 1.5)
        {
            camControl.maxVerticalAngle = degree;
        }

    }

    IEnumerator AlterCameraDistance(float time)
    {
        Debug.Log("zoom in");
        //calculate interpolation value
        float timeElapsed = 0;
        while (timeElapsed < 1)
        {
            // Calculate the interpolation factor based on the current distance of the camera
            float interpolationFactor = Mathf.InverseLerp(camControl.defeaultDistance,newCamDist, time);

            // Use the interpolation factor to adjust the time for interpolation
            float adjustedTime = interpolationFactor;

            if (newCamDist > defaultDistanceMin)
                camControl.defeaultDistance = Mathf.Lerp(camControl.defeaultDistance, newCamDist, timeElapsed);
            timeElapsed += Time.deltaTime * adjustedTime;
            yield return null;
        }
        isCoroutineRunning = false;
    }

    IEnumerator ReturnCamToNormal()
    {
        //Debug.Log("Zoom out");
        float timeElapsed = 0;
        while (timeElapsed < 1)
        {
                        // Calculate the interpolation factor based on the current distance of the camera
            float interpolationFactor = Mathf.InverseLerp(camControl.defeaultDistance, newCamDist, 1);

            // Use the interpolation factor to adjust the time for interpolation
            float adjustedTime = interpolationFactor;
            camControl.defeaultDistance = Mathf.Lerp(newCamDist, defaultDistanceMax, timeElapsed);
            timeElapsed += Time.deltaTime;
            yield return null;
        } 
    }
    /*
    [Header("Camera Obstruction")]
    public LayerMask obstructionMasks; // This layermask will be inverted to choose what layers to ignore. 
    Transform Obstruction;


    [Header("Camera Collision")]
    public LayerMask collisionMask;
    public bool isCollisionDetected = false;
    public float cameraSphereRadius = 0.3f;
    public float cameraCollisionOffset = 0.3f;
    public float minimumCollisionOffset = 0.3f;











    /* if (Physics.Raycast(ray, out hit, 40, ~(obstructionMasks),QueryTriggerInteraction.Ignore)) 
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
        }*/








    //      Obstruction = followObj.transform; // default starting point 
    /*

    #region Camera Collison

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

    */

}


