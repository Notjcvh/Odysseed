using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyValue : MonoBehaviour
{
    public Key keyScript;

    public int value;
    public string matchingTriggerName;
     void Awake()
    {
        value = keyScript.keyValue;
        matchingTriggerName = keyScript.triggerName;
        
    }



}



  
