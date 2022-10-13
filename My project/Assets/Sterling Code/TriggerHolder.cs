using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHolder : MonoBehaviour
{

    [Header("Refrencing")]
    public keyValue key1;
    public keyValue key2;
    public GameObject[] keysInPuzzle;
    public Transform[] triggers = null;

    private PlayerController player;

    [Header("Variables")]
    public int[] triggerValues = null;


    private void Start()
    {
        WithForeachLoop();
        key1 = GameObject.Find("Box (1)").GetComponent<keyValue>();
        key2 = GameObject.Find("Box (2)").GetComponent<keyValue>();
    }
    void WithForeachLoop()
    {
        //we're getting the children of this Game Object 
        triggers = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in transform)
        {
            //print("Foreach loop: " + child);
            if (gameObject.transform.parent != null)
                AssignValuesToTriggers();    
        }   
    }

    public void AssignValuesToTriggers()
    {
        for (int i = 0; i <triggers.Length; i++)
        {
            if (i == 0)
               triggerValues.SetValue(key1.value, 0);
            if (i == 1)
                triggerValues.SetValue(key2.value, 1);
        }
    }

    public void CompareValuesOfKeysNTriggers(GameObject other)
    {

        if (triggerValues[0] == key1.value)
            //if (gameObject.name == key1.matchingTriggerName)
                print("key and trigger match");
       else if(triggerValues[0] != key1.value)
           print("key and trigger don't match");


    }

}
