using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 

public class Input
{
    public KeyCode primary;


    public bool Pressed()
    {
        return UnityEngine.Input.GetKey(primary);
    }

    public bool PressedDown()
    {
        return UnityEngine.Input.GetKeyDown(primary);
    }

    public bool PressedUp()
    {
        return UnityEngine.Input.GetKeyUp(primary);
    }


}

public class PlayerInputs : MonoBehaviour
{
    public Input forward;
    public Input backward;
    public Input right;
    public Input left;
    public Input up;

    



    public int MoveAxisForward
    {
        get
        {

            if (forward.Pressed() && backward.Pressed()) { return 0; }
            else if (forward.Pressed()) { return 1; }
            else if (backward.Pressed()) { return -1; }
            else { return 0; }

        }

    }
    public int MoveAxisRight
    {
        get
        {
            if(right.Pressed() && left.Pressed()) { return 0; }
            else if(right.Pressed()) {return 1; }
            else if (left.Pressed()) { return -1; }
            else { return 0; }
        }
    }

    public int MoveAxisUp
    {
        get
        {
            if(up.Pressed()) { return 1; }
            else { return 0; }
        }
    }
    
    public const string MouseX = "Mouse X";
    public const string MouseY = "Mouse Y";
    public const string MouseScroll = "Mouse ScrollWheel";
    public const string Jump = "Jump";


    public static float mouseX  { get => UnityEngine.Input.GetAxis(MouseX); }
    public static float mouseY { get => UnityEngine.Input.GetAxis(MouseY); }
    public static float mouseScroll { get => UnityEngine.Input.GetAxis(MouseScroll); }    
    public static float jump { get => UnityEngine.Input.GetAxis(Jump); }





}
