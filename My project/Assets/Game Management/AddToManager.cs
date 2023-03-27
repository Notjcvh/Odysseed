using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToManager : MonoBehaviour
{

    private void Awake()
    {
        //Add this GameObject to List when it is created
        GameManager.instance.allObjects.Add(gameObject);
    }

    private void OnDestroy()
    {
        
    }
}
