using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHolder : MonoBehaviour
{

    [Header("Refrencing")]
    public GameObject player;
    public GameObject door;
    public Animation doorAnim;
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
      door = whichPuzzle.puzzle.transform.GetChild(0).gameObject;
      door.SetActive(true);
        doorAnim = door.GetComponent<Animation>();
    }

    public  void IsPuzzleComplete()
   {
     if(Input.GetKey(KeyCode.Y))
        {
            if (needMatchesToSolvePuzzle == totalvalue)
            {
                doorAnim.Play();

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
