using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
   
    void Start()
    {

        DamagePopUp.Create(transform.position, 200);
    }

}
