using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHolder : MonoBehaviour
{

    [Header("Refrencing")]
    public GameObject player;
    public PuzzleDataManager whichPuzzle;
    public Trigger trigger;


    public Animator myDoor = null;


    public int[] doorValues = null;
    


    public string doorOpen = "Door Open";
    int count = 0;


    public int needMatchesToSolvePuzzle;
    private void Start()
    {
        
      needMatchesToSolvePuzzle = whichPuzzle.keywords.Length * 2;
        doorValues = new int [whichPuzzle.keywords.Length];

    }
   public  void IsPuzzleComplete()
    {
     
        /*
        if (needMatchesToSolvePuzzle == number)
        {
            myDoor.Play(doorOpen, 0, 0);
            Debug.Log("Door Open");
        }
        else
        {
            
            Destroy(player);
        }*/
    }

    public void GetTriggeredValue(int number)
    {

        print(number);
        if(count < doorValues.Length)
        {
            doorValues.SetValue(number, count);
            count++;
        }
           

    }
 
   


 
}
