using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp : MonoBehaviour
{

    public List<Transform> childTransforms;
    public SceneHandeler sceneHandeler;
    private GameObject _player;
    public PlayerManger playerManger;
    public PlayerInput playerInput;
    public int currentActivePrompt;
    public bool activated = true;



    private void OnEnable()
    {
        sceneHandeler = GameObject.FindGameObjectWithTag("Scene Handler").GetComponent<SceneHandeler>();
        _player = GameObject.FindGameObjectWithTag("Player");
        playerManger = _player.GetComponent<PlayerManger>();
        playerInput = _player.GetComponent<PlayerInput>();
        childTransforms = new List<Transform>(this.transform.childCount);

        foreach (Transform child in transform)
        {
            childTransforms.Add(child);
        }
        foreach (Transform item in childTransforms)
        {
            item.GetComponent<Tutorial>().SetPlaceInIndex();
            item.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            activated = true;
            sceneHandeler.SetState(InteractionStates.Passive);
            sceneHandeler.DeactivatePlayer();
        }
    }


    // on Input press change the game 
    private void Update()
    {
        //Activate the current object based on the currentActivePrompt index
        for (int i = 0; i < childTransforms.Count; i++)
        {
            if (i == currentActivePrompt && activated)
            {
                childTransforms[i].gameObject.SetActive(true);
            }
            else
            {
                childTransforms[i].gameObject.SetActive(false);
            }
        }

        // Check for back button press
        if (playerManger.inactiveInputsEnabled)
        {
            if (playerInput.backButton && activated == true)
            {
                CloseTutorial(this);
            }



            if (playerInput.horizontalInput)
            {
                if (playerInput.horizontalAxis > 0)
                {
                    NextPrompt();
                }
                else if (playerInput.horizontalAxis < 0)
                {
                    LastPrompt();
                }
            }
        }
    }

    // Next Tutorial Prompt
    public void NextPrompt()
    {
        if (currentActivePrompt < childTransforms.Count - 1)
        {
            currentActivePrompt += 1;
        }
    }

    //Last Tutorial Prompt
    public void LastPrompt()
    {
        if (currentActivePrompt > 0)
        {
            currentActivePrompt -= 1;
        }
    }

    //close 
    public void CloseTutorial(TutorialPopUp obj)
    {
        obj.gameObject.SetActive(false);
        activated = false;
        childTransforms[currentActivePrompt].gameObject.SetActive(false);
        sceneHandeler.SetState(InteractionStates.Active);
        sceneHandeler.ActivatePlayer();
    }
}
