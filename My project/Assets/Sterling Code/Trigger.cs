using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [Header("Refrencing")]
    
    public TriggerHolder triggerHolder;
    public string[] tags = { "Player", "Key" };

    private void Start()
    {
        triggerHolder = GameObject.Find("Trigger Holder").GetComponent<TriggerHolder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //using a for loop to check for trigger tags in the string array
        for (int i = 0; i < tags.Length; i++)
        {
            if (other.CompareTag(tags[0]))
            {
               // Debug.Log("this is Player");
            }

            //beacause this runs twice 
            if (other.CompareTag(tags[1]))
            {
                triggerHolder.CompareValuesOfKeysNTriggers(this.gameObject);
                //Debug.Log("this is a key");
                // function that checks for a specifc int value 
            }
        }
    }
}
