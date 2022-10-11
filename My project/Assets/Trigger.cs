using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    [Header("Refrencing")]
    Key keyNumber;
    public GameObject key;
   


    public GameObject[] trigger = null;
    string[] tags = { "Player", "Key" };


    private void Start()
    {
        keyNumber = key.GetComponent<Key>();
        trigger = GameObject.FindGameObjectsWithTag("Key");
    
    }

    //throw this in the trigger script;

    // is something standing on the trigger
    
    private void OnTriggerEnter(Collider other)
    {
        //using a for loop to check for trigger tags in the string array
        for (int i = 0; i < tags.Length; i++)
        {
            if (other.CompareTag(tags[0]))
            {
                Debug.Log("this is Player");
            }
            if (other.CompareTag(tags[1]) )
            {
                Debug.Log("this is a key");
                CheckValuesOfKeys(gameObject);
                // function that checks for a specifc int value 
            }
        }
    }
    public void CheckValuesOfKeys(GameObject other)
    {
        int keynumber = keyNumber.keyNumber;
        Debug.Log(keynumber);


        for (int i = 0; i < trigger.Length; i++)
        {
            


        }
    }


    
}
