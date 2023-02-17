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

    protected Material material;
    

    private void Start()
    {
       // material = GetComponentInChildren<Material>();
        //check type if the door has an encounter the it's locked else then the door is unlocked
        if (encounterCollider != null)
        {
            print(this.name + " is locked? " + locked);
        }
        else
        {
            locked = false;
            print(this.name + " is locked? " + locked);
        }
    }
    public void UnlockDoors()
    {
        locked = false;
        material.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && locked == false)
        {
            door.SetBool("Open", true);
            door.SetBool("Close", false);
        }
           
    }
    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && locked == false)
        {
            door.SetBool("Close", true);
            door.SetBool("Open", false);

        }   
    }


}
