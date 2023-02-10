using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    
    public UnpackRoom myRoom;
    public BoxCollider triggerCollider; // this object, for backtracking 

    [Header("Animations")]
    [SerializeField] private Animator door;
    [SerializeField] private AnimationClip[] doorClips;

    [SerializeField] private bool locked = true; 
    public bool opened = false;
    public bool closed = false;

     public void UnlockDoor()
    {
        locked = false;
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && locked == false)
        {
            door.SetBool("Open", true);
            door.SetBool("Close", false);
        }
           
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && locked == false)
        {
            door.SetBool("Close", true);
            door.SetBool("Open", false);
        }
           
    }



}
