using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLock : MonoBehaviour
{
    public DoorHandler myParent;
    public Animator door;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && myParent.locked == false)
        {
            door.SetBool("Close", true);
            door.SetBool("Open", false);

            myParent.locked = true;
            this.gameObject.SetActive(false);
          //  myParent.material.SetColor("_Color", Color.red);
        }
    }

}
