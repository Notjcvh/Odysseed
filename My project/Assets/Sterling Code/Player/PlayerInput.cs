using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 movementInput {get; private set; }
    public bool jumpInput {get; private set; }
    public bool pickUp { get; private set; }
    public bool drop { get; private set; }

    public bool attack { get; private set; }



    private void Update()
    {
 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movementInput = new Vector3(horizontal, 0, vertical).normalized;

        jumpInput = Input.GetButtonDown("Jump");

        pickUp = Input.GetKeyDown(KeyCode.Mouse2);
        drop = Input.GetKeyUp(KeyCode.Mouse2);

        attack = Input.GetKeyDown(KeyCode.Mouse0);

    }
}
