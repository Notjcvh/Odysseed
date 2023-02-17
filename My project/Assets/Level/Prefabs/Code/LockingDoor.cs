using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockingDoor : DoorHandler
{
    private GameObject nearestEncounterTrigger;

    private GameObject[] neareastEncounters;





    public override void OnTriggerExit(Collider other)
    {
        // make sure lock is false
        locked = false;

        base.OnTriggerExit(other);
        if (other.gameObject.CompareTag("Player") && locked == false)
        {
            door.SetBool("Close", true);
            door.SetBool("Open", false);

            locked = true;
            print(this.name + " is locked? " + locked);
            GetClosestEncounterCollider();
            

        }
    }

    void GetClosestEncounterCollider()
    {
        float closestDistanceRange = 50;
        neareastEncounters = GameObject.FindGameObjectsWithTag("Room");
        for (int  objects = 0;  objects < neareastEncounters.Length;  objects++)
        {
            float distance = Vector3.Distance(neareastEncounters[objects].transform.position, this.transform.position);
            if (distance < closestDistanceRange)
            {
                nearestEncounterTrigger = neareastEncounters[objects];
                print(nearestEncounterTrigger);
            }

        }
    }

    void ChangeColor()
    {

        material.color = Color.red;       
    }


}
