using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class CameraCollision : MonoBehaviour
{
    //Reference to Camera Controller 
    private CameraController camControl;

    private int groundLayer = (1 << 8);
    private int wallLayer = (1 << 10);



    //Methods
    [Header("Camera Collision")]







    //public LayerMask collisionMask;
    public bool isCollisionDetected = false;




    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 down = new Vector3(0, -10, 0);
        //  Gizmos.DrawLine(transform.position, transform.position + down);

    }

    private void Start()
    {
        camControl = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraController>();

    }


    private void OnCollisionEnter(Collision collision)
    {

        if (groundLayer == (groundLayer & (1 << collision.gameObject.layer)))
        {
            isCollisionDetected = true;
        }

        else if (wallLayer == (wallLayer & (1 << collision.gameObject.layer)))
        {
            Debug.Log(Convert.ToString(wallLayer, 2).PadLeft(32, '0'));
            Debug.Log(Convert.ToString(collision.gameObject.layer, 2).PadLeft(32, '0'));
            Debug.Log("Hit wall");
        }


    }


    private void OnCollisionStay(Collision collision)
    {
        // check distacne to ground
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(downRay, out hit, groundLayer))
        {

            int roundedDist = Mathf.CeilToInt(hit.point.y);
            Debug.DrawLine(transform.position, new Vector3(transform.position.x, roundedDist, transform.position.z), Color.green);
            // print("This is the hit point " + transform.position + " This is the hit point" + roundedDist);
            if (transform.position.y < roundedDist + .5f)
            {
                print("too low");

            }
        }



    }

    private void OnCollisionExit(Collision collision)
    {

        if (groundLayer == (groundLayer & (1 << collision.gameObject.layer)))
        {
            isCollisionDetected = false;
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


