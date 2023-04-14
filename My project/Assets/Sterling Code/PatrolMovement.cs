using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PatrolMovement : MonoBehaviour
{
    public Transform patrolPath; //GamObject that contains the path points
    public float minimumReachDistance = 0.01f; // how close the hazard shoul be beofre moving to the nex point
    public float movementSpeed = 1; //how fast the object will move between points
    int targetIndex = 0;
    public VisualEffect vfx;


   

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, patrolPath.GetChild(targetIndex).position, movementSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, patrolPath.GetChild(targetIndex).position) < minimumReachDistance)
        {
            targetIndex++;
            if (targetIndex >= patrolPath.childCount) targetIndex = 0;
        }

      


    }
}
