using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
   
    void  Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
            DamagePopUp.Create(transform.position, 200);
            
    }

}
