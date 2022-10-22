using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [Header("Refrencing")]
    
    public TriggerHolder triggerHolder;
    //public TriggerSettings triggerSettings;
    public PuzzleDataManager whichPuzzle;

    public string[] tags = { "Player", "Key" };

    public bool keyNoLongerOnTrigger = false;


    private HashSet<GameObject> takenDamageFrom = new HashSet<GameObject>();


     int numberOfCorrectMatchesInPuzzle;

    private void Start()
    {
        triggerHolder = GameObject.Find("Trigger Holder").GetComponent<TriggerHolder>();

    }
    

    private void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag(tags[0]))
            {
               // Debug.Log("this is Player");
            }

            if (other.CompareTag(tags[1]))
            {
                takenDamageFrom.Add(other.gameObject);
                CompareCollisionStrings(this.gameObject.name, other.name);
               
            }
      
    }

    private void OnTriggerExit(Collider other)
    {
        //using a for loop to check for trigger tags in the string array
       
   
            if (other.CompareTag(tags[0]))
            {
                // Debug.Log("this is Player");
            }

            if (other.CompareTag(tags[1]))
            {
               
                numberOfCorrectMatchesInPuzzle = - 1;
                triggerHolder.GetTriggeredValue(numberOfCorrectMatchesInPuzzle);
            }
      
    }

    public void CompareCollisionStrings(string trigger, string key)
    {
        string givenKeyword = trigger + key;


        foreach (string part in whichPuzzle.keywords)
        {

            if (string.Equals(part, givenKeyword))
            {
               
                numberOfCorrectMatchesInPuzzle = 1;
                triggerHolder.GetTriggeredValue(numberOfCorrectMatchesInPuzzle);
            }
        }
    }


}
