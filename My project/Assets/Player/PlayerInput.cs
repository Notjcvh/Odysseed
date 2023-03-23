using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Paused and Settings 



    //InGame Player 
    public Vector3 movementInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool attack { get; private set; }
    public bool secondaryAttack { get; private set; }

    public bool chargedSecondaryAttack { get; private set;  }


    public bool dash { get; private set; } // shift

    public bool anyKeyFrame { get; private set; } // shift

    public bool target { get; private set; }

    public float mouseX { get; private set; }
    public float mouseY { get; private set; }

    public bool interact { get; private set; }

    public bool activateSpecial { get; private set; }

    public bool activateUltimate { get; private set; }

    public bool activateSeedWheel { get; private set; }


    /// <summary>
    /// e to interact 
    /// q to sepcial ability 
    /// r ultimate 
    /// tab seed wheel
    /// 
    /// </summary>

    private void Update()
    {
        ///  <summary>
        ///  All Input configurations can be found in the Input Manager.
        ///  Edit --> Project Settings --> Input Manager
        ///  All Inputs will be labeled by number of where they appear
        /// </summary>
        

        //Basic Movement 
        float horizontal = Input.GetAxis("Horizontal"); // number 1
        float vertical = Input.GetAxis("Vertical"); // number 2
        movementInput = new Vector3(horizontal, 0, vertical).normalized;

        //Attacking 
        attack = Input.GetMouseButtonDown(0);
        secondaryAttack = Input.GetMouseButtonUp(1);
        chargedSecondaryAttack = Input.GetMouseButton(1);

        //Jumping 
        jumpInput = Input.GetButton("Jump");// number 6

        // Camera Movement 
        mouseX = Input.GetAxisRaw("Mouse X"); // number 7
        mouseY = Input.GetAxisRaw("Mouse Y"); // number 8

        dash = Input.GetButton("Dash"); // number 17

        target = Input.GetButton("Target"); // Mouse 3

        interact = Input.GetButtonDown("Interact"); // number 19

        activateSpecial = Input.GetButton("Activate Special"); // number 20

        activateUltimate = Input.GetButton("Activate Ultimate"); // number 21

        activateSeedWheel = Input.GetButtonDown("Activate Seed Wheel"); // number 22


        
    }
}
