using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHolder : MonoBehaviour
{

    [Header("Refrencing")]
    public GameObject player;
    public PuzzleDataManager whichPuzzle;
    public AnimationClip[] clips;
  
    public NPC puzzleDialouge;
    public string[] tags = { "Player", "Ally"};

    [Header("Animations")]
    public Animator door = null;
    public GameObject doorAniamtionTrigger;

    [Header("Variables")]
    public int needMatchesToSolvePuzzle;
    public int totalvalue;



    private void Start()
    {
        needMatchesToSolvePuzzle = whichPuzzle.keywords.Length;
        door = doorAniamtionTrigger.GetComponent<Animator>();
       
    }
    public void IsPuzzleComplete()
   {
      
        if (needMatchesToSolvePuzzle == totalvalue)
            {
                puzzleDialouge.puzzleCompleted = true; 
                door.Play(clips[0].name, 0, 0);

            // I want this to be able to wait before activating the Door Open Animation 

        }
        else
        {
             Destroy(player.gameObject);

       
        }
    }


    public void GetTriggeredValue(int number)
    {
        print(number);
        totalvalue += number;
    }

    private void CloseDoor()
    {
      door.Play(clips[1].name, 0, 0);

    }
    //Door Detector Reciver Functions
    public void WhenTriggerEnter()
    {
        IsPuzzleComplete();
    }
    public void WhenTriggerExit()
    {
        CloseDoor();
    }
      

}
