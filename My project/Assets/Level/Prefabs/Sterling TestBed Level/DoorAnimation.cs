using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public int doorValue = 1;

    public UnpackRoom myRoom;

    [Header("Animations")]
    [SerializeField] private Animator door;
    [SerializeField] private AnimationClip[] doorClips;
    public BoxCollider boxCollider;
 
    public bool active;
    public bool opened = false;
    public bool closed = false;

    private void Awake()
    {
        boxCollider.enabled = false;
    }

    public void UnlockDoor()
    {
        this.GetComponent<Animator>().Play(doorClips[0].name, 0, 0);
        opened = true;
        boxCollider.enabled = true;
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
