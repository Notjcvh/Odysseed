using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHolder : MonoBehaviour
{

    [Header("Refrencing")]
    Key keyNumber;
    public GameObject key;
   


    public Transform[] triggers = null;
    public string[] tags = { "Player", "Key" };


    private void Start()
    {
        //keyNumber = key.GetComponent<Key>();

        WithForeachLoop();
    }

    //throw this in the trigger script;

    // is something standing on the trigger

    void WithForeachLoop()
    {
        //we're getting the children of this Game Object 
        triggers = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in transform)
        {
            print("Foreach loop: " + child);
            if (gameObject.transform.parent != null)
            {
                print(triggers.Length);
            }
        }   
    }

    public void CheckValuesOfKeys(GameObject other)
    {
       
    }


    
}
