using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PickUp : MonoBehaviour
{
    private PlayerInput playerInput;

    public GameObject pickUpObject = null;
    public Transform holdingPosition;
    public Rigidbody playerBody;
    public LayerMask CanPickUp;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {/*
        if (playerInput.pickUp)
            Pickup();
        else if (playerInput.drop)       
            Drop();*/
    }
    private void Pickup()
    { 
      RaycastHit hit;
      if (Physics.Raycast(playerBody.transform.position, playerBody.transform.forward, out hit, 3, CanPickUp))
      {
           pickUpObject = hit.collider.gameObject;
           pickUpObject.GetComponent<Rigidbody>().useGravity = false;
           pickUpObject.transform.position = holdingPosition.position ;
           pickUpObject.transform.parent = GameObject.Find(holdingPosition.gameObject.name).transform;
      }
    }
    private void Drop()
    {
        holdingPosition.DetachChildren();
        pickUpObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
