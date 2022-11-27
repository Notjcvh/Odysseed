using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boneyard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        
        Destroy(obj);

    }
}
