using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHolder : MonoBehaviour
{

    [Header("Refrencing")]
    public GameObject player;
    public PuzzleDataManager whichPuzzle;
    public GameObject[] keysInPuzzle;

    public Animator myDoor = null;

    public Transform[] triggers = null;
    


    public string doorOpen = "Door Open";




    public int numberOfCorrectMatchesInPuzzle;
    public int needMatchesToSolvePuzzle;
    private void Start()
    {
        
       needMatchesToSolvePuzzle = whichPuzzle.keywords.Length * 2;

    }
    void IsPuzzleComplete()
    {
        if (needMatchesToSolvePuzzle == numberOfCorrectMatchesInPuzzle)
        {
            myDoor.Play(doorOpen, 0, 0);
            Debug.Log("Door Open");
        }
        else
        {
            
            Destroy(player);
        }
    }
 
    public void CompareCollisionStrings(string trigger, string key)
    {
        string givenKeyword = trigger + key;


        foreach (string part in whichPuzzle.keywords)
        {

            if (string.Equals(part, givenKeyword))
            {
                print("Match");
                numberOfCorrectMatchesInPuzzle += 1;
                IsPuzzleComplete();
            }               
        }
    }



 
}
