using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Paused and Settings 



    //InGame
    public Vector3 movementInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool attack { get; private set; }
    public bool secondaryAttack { get; private set; }
    public bool changeWeaponState { get; private set; }
    public bool dash { get; private set; } // shift



    /// <summary>
    /// e to interact 
    /// q to sepcial ability 
    /// r ultimate 
    /// tab seed wheel
    /// 
    /// </summary>

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movementInput = new Vector3(horizontal, 0, vertical).normalized;
        jumpInput = Input.GetButtonDown("Jump");
        attack = Input.GetButton("Primary Attack");
        secondaryAttack = Input.GetButton("Secondary Attack");
        dash = Input.GetButton("Dash");
    }
}
