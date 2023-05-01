using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractableObj : MonoBehaviour, IAssignable
{
    public GameObject player;
    public PlayerEventsWithData playerEvent;
    public bool calledEvent;
    public bool hasInteractedWith;
    public bool playerInRange;

    private float _distanceToPlayer;
    public float interactRange;
    public GameObject interactableIndicator;
    private IndicatorScript indicator;
    public GameObject[] indicators = new GameObject[2];

    public GameObject prompt; // prompt can be a tutorial or a dialouge 

    [TextArea]
    public string objective;

    // Start is called before the first frame update
    void Start()
    {
        hasInteractedWith = false;
        player = GameObject.FindGameObjectWithTag("Player");
        indicator = interactableIndicator.GetComponent<IndicatorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        interactableIndicator.SetActive(!hasInteractedWith);
        _distanceToPlayer = Mathf.Abs(Vector3.Distance(transform.position, player.transform.position));
        if (_distanceToPlayer < interactRange)
        {
            playerInRange = true;
            indicators[0].SetActive(false);
            indicators[1].SetActive(true);
            indicator.changeScale = false; // stop animation 
        }
        else
        {
            playerInRange = false;
            indicators[0].SetActive(true);
            indicators[1].SetActive(false);
            indicator.changeScale = true; // start animation
        }
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !hasInteractedWith)
        {
            //Do something
            InteractFunction();
        }

        if(hasInteractedWith && calledEvent == false && prompt != null && prompt.activeInHierarchy == false)
        {
            if (playerEvent != null)
            {
                //Assign the scriptable object event string with the our objective string 
                playerEvent.text = objective;
                AssignObjective(playerEvent);
            }
            calledEvent = true;
        }
        else if( hasInteractedWith  && calledEvent == false)
        {

            if (playerEvent != null)
            {
                //Assign the scriptable object event string with the our objective string 
                playerEvent.text = objective;
                AssignObjective(playerEvent);
            }
            calledEvent = true;
        }
    }

    void InteractFunction()
    {
        if(prompt != null)
        {
            //pull up the Tutorial Prompt
            prompt.SetActive(true);
            prompt.GetComponent<TutorialPopUp>().Activated();
        }
        hasInteractedWith = true;
    }

    public void AssignObjective(PlayerEventsWithData playerEvent)
    {
        playerEvent.Raise();
    }
}

public interface IAssignable
{
    void AssignObjective(PlayerEventsWithData assignObjective);
}