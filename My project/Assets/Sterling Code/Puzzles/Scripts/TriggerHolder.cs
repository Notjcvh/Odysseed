using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHolder : MonoBehaviour
{

    [Header("Refrencing")]
    public GameObject player;
    public PuzzleDataManager whichPuzzle;
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
     if(Input.GetKey(KeyCode.Y))
     {
            if (needMatchesToSolvePuzzle == totalvalue)
            {
                puzzleDialouge.puzzleCompleted = true;
                //door.Play("Door Open", 0, 0); 

            }
            else
            {
                Destroy(player.gameObject);

            }
        }
    }

    public void GetTriggeredValue(int number)
    {

        print(number);
        totalvalue += number;
    }



    // Might want to switch this to a coroutine that begins when the puzzle starts and ends when the puzzle is finished

    private void Update()
    {
        IsPuzzleComplete();
    }



}
