using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject pickUpObject = null;
    public Transform holdingPosition;
    public Rigidbody playerBody;
    public LayerMask CanPickUp;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Pickup();
            
        }

        else if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            Drop();
        }
    }



    private void Pickup()
    {     
      RaycastHit hit;
      if (Physics.Raycast(playerBody.transform.position, playerBody.transform.forward, out hit, 3, CanPickUp))
      {
           pickUpObject = hit.collider.gameObject;
           pickUpObject.GetComponent<Rigidbody>().useGravity = false;
           pickUpObject.transform.position = holdingPosition.position;
           pickUpObject.transform.parent = GameObject.Find("HoldingDestination").transform;
      }
    }
    private void Drop()
    {
        holdingPosition.DetachChildren();
        pickUpObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
