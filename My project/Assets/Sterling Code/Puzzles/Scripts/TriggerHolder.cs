using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHolder : MonoBehaviour
{

    [Header("Refrencing")]
    public GameObject player;

    public Animator doorS = null;
    
    

    public PuzzleDataManager whichPuzzle;
    public Trigger trigger;

    public string[] tags = { "Player", "Ally"};

    [Header("Animations")]
   
    
  

    [Header("Variables")]
    public int needMatchesToSolvePuzzle;
    public int totalvalue;
    private void Start()
    {
        
      needMatchesToSolvePuzzle = whichPuzzle.keywords.Length;   
    }

    public  void IsPuzzleComplete()
   {
     if(Input.GetKey(KeyCode.Y))
        {
            if (needMatchesToSolvePuzzle == totalvalue)
            {
                doorS.Play("Door Open ", 0, 0); 

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
