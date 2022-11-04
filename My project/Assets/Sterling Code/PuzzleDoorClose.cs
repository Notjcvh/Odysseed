using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoorClose : MonoBehaviour
{

    Animator door;

    private void Start()
    { 
        door = GetComponent<Animator>();

    }


    private void OnTriggerEnter(Collider other)
    {
        door.Play("Door Close", 0, 0);
     
    }
}
