using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [Header("Refrencing")]
    
    public TriggerHolder triggerHolder;
    public TriggerSettings triggerSettings;
    public string[] tags = { "Player", "Key" };


    public string keyName;

    private void Start()
    {
        triggerHolder = GameObject.Find("Trigger Holder").GetComponent<TriggerHolder>();
        keyName = triggerSettings.keyName;
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

            if (other.CompareTag(tags[1]))
            {
                if (other.name == keyName)
                {
                    print("These match send message to trigger Holder");
                }
                else
                    print("these don't match");
            }
        }
    }
}
