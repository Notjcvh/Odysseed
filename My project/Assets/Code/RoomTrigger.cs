using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [Header("References")]
    private CameraController cam;
    [SerializeField] GameObject player;

    private void Start()
    {
        cam = player.GetComponent<CameraController>();
       
    }


    private void OnTriggerEnter(Collider other)
    {
        cam.camPriority = 1;
     
    }

    private void OnTriggerExit(Collider other)
    {
        cam.camPriority = 0;
    }

}
