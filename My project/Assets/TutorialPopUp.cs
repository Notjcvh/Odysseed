using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp : MonoBehaviour
{

    public List<Transform> childTransforms;
    public SceneHandler sceneHandler;
    private GameObject _player;
    public PlayerManger playerManger;
    public PlayerInput playerInput;
    public int currentActivePrompt;
    public bool activated = true;

    private void OnEnable()
    {
        sceneHandler = GameObject.FindGameObjectWithTag("Scene Handler").GetComponent<SceneHandler>();
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
            item.GetComponent<Tutorial>().Set();
            item.gameObject.SetActive(false);
        }
    }

    //If the player triggers the tutorial use this
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            activated = true;
            sceneHandler.SetState(InteractionStates.Passive);
           // sceneHandler.DeactivatePlayer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            activated = true;
            sceneHandler.SetState(InteractionStates.Passive);
            // sceneHandler.DeactivatePlayer();
        }
    }

    //else call this 
    public void Activated()
    {
        this.gameObject.SetActive(true);
        activated = true;
        sceneHandler.SetState(InteractionStates.Passive);
        sceneHandler.DeactivatePlayer();
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
                CloseTutorial(this.gameObject);
            }


            if (playerInput.horizontalInput && activated == true)
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
    public void CloseTutorial(GameObject obj)
    {
        obj.SetActive(false);
        activated = false;
        childTransforms[currentActivePrompt].gameObject.SetActive(false);
        sceneHandler.SetState(InteractionStates.Active);
        sceneHandler.ActivatePlayer();
    }
}
