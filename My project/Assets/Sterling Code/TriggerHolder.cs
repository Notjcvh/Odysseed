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
            if (gameObject.transform.parent != null) return;
        }   
    }

   

}
