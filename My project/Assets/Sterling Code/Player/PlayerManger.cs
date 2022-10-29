using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManger : MonoBehaviour
{
    public PlayerStats stats;

    public ElementType element;

    


    private void Update()
    {
       
        if(Input.GetKeyDown(KeyCode.E))
        {
            WeaponStateForward(element);
        }

    }

    void WeaponStateForward(ElementType state)
    {
       
        print(state);
        if(state == ElementType.Base)
        {
            Debug.Log("Switch");
            element = ElementType.Water;
        }
        if(state == ElementType.Water)
        {
            Debug.Log("Switch");
            element = ElementType.Fire;
        }
        if (state == ElementType.Fire)
        {
            Debug.Log("Switch");
            element = ElementType.Earth;
        }
        if(state == ElementType.Earth)
        {
            Debug.Log("Switch");
            element = ElementType.Base;
        }

    }

}


public enum ElementType
{
    Base = 0,
    Water = 1,
    Fire = 2,
    Earth = 3,
    Air = 4
}


