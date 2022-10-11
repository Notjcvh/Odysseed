using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : TriggerHolder
{

    private void OnTriggerEnter(Collider other)
    {
        //using a for loop to check for trigger tags in the string array
        for (int i = 0; i < tags.Length; i++)
        {
            if (other.CompareTag(tags[0]))
            {
               // Debug.Log("this is Player");
            }
            if (other.CompareTag(tags[1]))
            {
                //Debug.Log("this is a key");
                CheckValuesOfKeys(gameObject);
                // function that checks for a specifc int value 
            }
        }
    }
}
