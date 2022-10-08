using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{

    [SerializeField] UnityEvent OnUnlock;

    float objectWeight;
    float triggerweight;
    bool correctWeight;
    public TriggerSettings trigger;
    

    private void Start()
    {
        triggerweight = trigger.weight;

    }
    public void UnlockEventTriggered()
    {
        OnUnlock.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject)
        {
            if(correctWeight)
            {
                UnlockEventTriggered();
            }
           
        }
    }

    private void CheckWeight()
    {
        if (objectWeight < triggerweight)
            return;
        else if(objectWeight >= triggerweight)
        {
            correctWeight = true;
        }
           
        //check value 

    }

    public void TestUnlock()
    {
        Debug.Log("Goal");
    }




}



