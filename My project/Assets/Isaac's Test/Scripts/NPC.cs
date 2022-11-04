using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPC : MonoBehaviour
{
    [SerializeField] private Transform player;
    private PlayerMovement playerController;
    private NavMeshAgent navMeshAge;
    private bool isTalking;
    private int dialoguePointer;
    public GameObject dialogue;
    private TextMeshProUGUI myDialouge;
    public string[] dialogueList;

    public bool person;
    public bool puzzleCompleted;
    public float distanceFromPlayer;
    public float talkRange;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        isTalking = false;
        myDialouge = dialogue.GetComponentInChildren<TextMeshProUGUI>();
        playerController = player.GetComponent<PlayerMovement>();
    }
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);
        dialogue.SetActive(isTalking);
        //playerController.isTalking = isTalking;
        if (Input.GetKeyDown(KeyCode.T) && distanceFromPlayer < talkRange && !person)
        {
            if (!isTalking)
            {
                StartDialouge();
            }

        }
        if (puzzleCompleted && distanceFromPlayer < talkRange)
        {
            if (!isTalking)
            {
                StartDialouge();
            }
        }
        if(isTalking && Input.GetKeyDown(KeyCode.T))
        {
            dialoguePointer++;
            if (dialoguePointer == dialogueList.Length)
            {
                EndDialouge();
            }
            myDialouge.text = dialogueList[dialoguePointer];
        }
    }
    public void StartDialouge()
    {
        isTalking = true;
        dialoguePointer = 0;
    }
    public void EndDialouge()
    {
        isTalking = false;
        dialoguePointer = 0;
    }


}
