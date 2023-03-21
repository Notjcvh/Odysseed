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


    public float contactPoint;


    //Methods
    [Header("Camera Collision")]
    //public LayerMask collisionMask;
    public bool isHittingWall = false;

    void OnDrawGizmos()
    {
        //  Vector3 direct = Vector3.Normalize(camControl.followObj.position - transform.position);
        // float distance = Mathf.Ceil((transform.position - camControl.followObj.position).magnitude);
        //print(distance);
        //Gizmos.DrawRay(transform.position, direct);

        if(isHittingWall == true)
        {
            Gizmos.color = Color.green;
            Vector3 followObjUp = Vector3.up;
            Gizmos.DrawRay(camControl.followObj.position, followObjUp);
            Gizmos.color = Color.blue;
            Vector3 directtoCam = Vector3.Normalize(transform.position - camControl.followObj.position);
            Gizmos.DrawRay(camControl.followObj.position, directtoCam);
        }
    }
    private void Start()
    {
        camControl = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraController>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Run the checks 
        if (groundLayer == (groundLayer & (1 << collision.gameObject.layer)))
        {
            isHittingWall = true;
        }

        else if (wallLayer == (wallLayer & (1 << collision.gameObject.layer)))
        {
            Debug.Log(Convert.ToString(wallLayer, 2).PadLeft(32, '0'));
            Debug.Log(Convert.ToString(collision.gameObject.layer, 2).PadLeft(32, '0'));
            Debug.Log("Hit wall");
            camControl.defeaultDistance -= 1;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if(isHittingWall == true)
        {
            RaycastHit hit;
            Ray downRay = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(downRay, out hit, groundLayer))
            {
                #region  Checking For Distance from Ground
                //B
                Vector3 direct = Vector3.Normalize(transform.position - camControl.followObj.position);
                float distancePlayerToCam = (transform.position - camControl.followObj.position).magnitude;
                //A
                float distanceToGround = (transform.position - hit.point).magnitude; 
                //C
                Vector3 directionToGround = Vector3.Normalize(hit.point - camControl.followObj.position);
                float distancePlayerToGround = (hit.point - camControl.followObj.position).magnitude;
            
                float angleInRadians = Mathf.Atan(distanceToGround / distancePlayerToCam); //arctan(a/b
                float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

                //CamController
                  Debug.DrawRay(camControl.followObj.position, direct * distancePlayerToCam, Color.blue);
                  Debug.DrawRay(transform.position, Vector3.down * distanceToGround, Color.green);
                  Debug.DrawRay(camControl.followObj.position, directionToGround * distancePlayerToGround, Color.red);

                if (angleInDegrees < 2)
                {
                   // print("too low");
                    HandleGroundCollision(hit);
                }
                #endregion
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
       // return to default values 
    }

    void HandleGroundCollision(RaycastHit hit)
    {    
        Vector3 followObjUp = camControl.followObj.up;
        Vector3 directtoCam = Vector3.Normalize(transform.position - camControl.followObj.position);
   
        float dot = Vector3.Dot(directtoCam, followObjUp);
        dot = Mathf.Clamp(dot, -1, 1);

        float angleRad = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float degree = camControl.maxVerticalAngleRef - angleRad;
      //  print(degree);
        if(degree <= -14)
        {
            camControl.minVertivalAngleRef = -14;
         //   Debug.Log(camControl.minVertivalAngleRef);
            StartCoroutine(camControl.ReturnCamToDefalut());
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


