using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Paused and Settings 

    public bool pause { get; private set; }


    //InGame Player 
    public Vector3 movementInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool attack { get; private set; }
    public bool secondaryAttack { get; private set; }
    public bool chargedSecondaryAttack { get; private set;  }
    public bool dash { get; private set; } // shift
    public bool target { get; private set; }
    public float mouseX { get; private set; }
    public float mouseY { get; private set; }
    public bool interact { get; private set; }
    public bool activateWaterSpecial { get; private set; }
    public bool activateEarthSpecial { get; private set; }
    public bool activateSunSpecial { get; private set; }

    /// <summary>
    /// e to interact 
    /// q to sepcial ability 
    /// r ultimate 
    /// tab seed wheel
    /// </summary>

    private void Update()
    {
        ///  <summary>
        ///  All Input configurations can be found in the Input Manager.
        ///  Edit --> Project Settings --> Input Manager
        ///  All Inputs will be labeled by number of where they appear
        /// </summary>


        // Camera Movement 
        mouseX = Input.GetAxisRaw("Mouse X"); // number 7
        mouseY = Input.GetAxisRaw("Mouse Y"); // number 8

        //Basic Movement
        float horizontal = Input.GetAxis("Horizontal"); // number 1
        float vertical = Input.GetAxis("Vertical"); // number 2
        movementInput = new Vector3(horizontal, 0, vertical).normalized;

        //Jumping 
        jumpInput = Input.GetButtonDown("Jump");// number 6
    
        //Dashing
        dash = Input.GetButtonDown("Dash"); // number 17

        //Attacking 
        attack = Input.GetMouseButtonDown(0);
        secondaryAttack = Input.GetMouseButtonUp(1);
        chargedSecondaryAttack = Input.GetMouseButton(1);

        //Targeting
        target = Input.GetButton("Target"); // Mouse 3

        //Interact
        interact = Input.GetButtonDown("Interact"); // number 19

        //Water Special 
        activateWaterSpecial = Input.GetButton("Activate Water Special"); // number 20

        activateEarthSpecial = Input.GetButton("Activate Earth Special"); // number 21

        activateSunSpecial = Input.GetButtonDown("Activate Sun Special"); // number 22


        //Pausing
        pause = Input.GetButtonDown("Pause");
    }
}
