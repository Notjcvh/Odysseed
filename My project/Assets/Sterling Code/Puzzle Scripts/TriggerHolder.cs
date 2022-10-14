using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHolder : MonoBehaviour
{

    [Header("Refrencing")]
    public keyValue key1;
    public keyValue key2;
    public PuzzleDataManager whichPuzzle;
    public GameObject[] keysInPuzzle;
    public Transform[] triggers = null;



    public int numberOfCorrectMatchesInPuzzle;
    public int needMatchesToSolvePuzzle;
    private void Start()
    {
        
        key1 = GameObject.Find("Box (1)").GetComponent<keyValue>();
        key2 = GameObject.Find("Box (2)").GetComponent<keyValue>();

        needMatchesToSolvePuzzle = whichPuzzle.keywords.Length * 2;
    }


 
    void IsPuzzleComplete()
    {
        if (needMatchesToSolvePuzzle == numberOfCorrectMatchesInPuzzle)
        {
            print("Puzzle is Solved");
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
