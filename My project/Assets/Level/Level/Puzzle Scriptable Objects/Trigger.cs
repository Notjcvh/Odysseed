using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [Header("Refrencing")]

    public GameObject puzzleHolder;
    public PuzzleRoom whichPuzzleInScene;
    public PuzzleDataManager whichPuzzle;

    public string[] tags = { "Player", "Key" };

    public bool keyNoLongerOnTrigger = false;

     int numberOfCorrectMatchesInPuzzle;

    private void Start()
    {
        whichPuzzleInScene = puzzleHolder.gameObject.GetComponent<PuzzleRoom>();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag(tags[0]))
            {
               // Debug.Log("this is Player");
            }

            if (other.CompareTag(tags[1]))
            {
                
                CompareCollisionStrings(this.gameObject.name, other.name);
                Debug.Log(this.gameObject.name);
            Debug.Log(other.name);
               
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
                whichPuzzleInScene.GetTriggeredValue(numberOfCorrectMatchesInPuzzle);
            }
      
    }

    public void CompareCollisionStrings(string trigger, string key)
    {
        string givenKeyword = trigger + ' ' +key;


        foreach (string part in whichPuzzle.keywords)
        {

            if (string.Equals(part, givenKeyword))
            {
               
                numberOfCorrectMatchesInPuzzle = 1;
                whichPuzzleInScene.GetTriggeredValue(numberOfCorrectMatchesInPuzzle);
            }
        }
    }


}
