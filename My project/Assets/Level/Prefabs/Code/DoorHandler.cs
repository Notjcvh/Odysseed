using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public GameObject encounterCollider; // this object, for backtracking 

    [Header("Animations")]
    public Animator door;
    public  AnimationClip[] doorClips;

    public bool locked = true;
    public Material material;


    private void Start()
    {
        material = GetComponentInChildren<MeshRenderer>().material;
        if (locked != true)
        {
            material.SetColor("_Color", Color.green);
        }
    }
    public virtual void UnlockDoors()
    {
        locked = false;
        material.SetColor("_Color", Color.green);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && locked == false)
        {
            Open();
        }
    }
    public virtual void Open()
    {
        door.SetBool("Open", true);
        door.SetBool("Close", false);
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
