using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockingDoor : DoorHandler
{
    //this Door Handler will reference two Box Colliders, 
    //First for opening and closing 
    public BoxCollider collider1;
    //Second for locking 
    public BoxCollider collider2;

    public override void Open()
    {
        base.Open();
        if(encounterCollider.GetComponent<CombatRoom>().isRoomComplete != true)
        {
            collider1.enabled = false;
        }
    }

    public override void UnlockDoors()
    {
        base.UnlockDoors();
        collider1.enabled = true;
        collider2.enabled = false;
    }
}
