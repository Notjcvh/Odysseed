using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    
    public UnpackRoom myRoom;
    public BoxCollider triggerCollider; // this object, for backtracking 

    [Header("Door Values")]
    public int doorValue; // for operating rooms with multiple doors  


    [Header("Animations")]
    [SerializeField] private Animator door;
    [SerializeField] private AnimationClip[] doorClips;
   
    
    public bool opened = false;
    public bool closed = false;

    private void Awake()
    {
        triggerCollider.enabled = false;
    }

    public void UnlockDoor()
    {
        this.GetComponent<Animator>().Play(doorClips[0].name, 0, 0);
        triggerCollider.enabled = true;
        opened = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(opened == false)
        {
            door.Play(doorClips[0].name, 0, 0);
            opened = true;
            closed = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(closed == false)
        {
            door.Play(doorClips[1].name, 0, 0);
            closed = true;
            opened = false;
        }
        if(closed == true && opened == true)
        {
            door.Play(doorClips[1].name, 0, 0);
            closed = true;
            opened = false;  
        }
    }



}
