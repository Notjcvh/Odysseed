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
    private GameObject talkingIndicator;
    private bool isTalking;
    private int dialoguePointer;
    public bool hasTalked;
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
        talkingIndicator = GameObject.FindGameObjectWithTag("TalkingIndicator");
        hasTalked = false;
    }
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);
        dialogue.SetActive(isTalking);
        talkingIndicator.SetActive(!hasTalked);
        //playerController.isTalking = isTalking;
        if (Input.GetKeyDown(KeyCode.E) && distanceFromPlayer < talkRange && !person)
        {
            if (!isTalking)
            {
                StartDialouge();
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && puzzleCompleted && distanceFromPlayer < talkRange)
        {
            if (!isTalking)
            {
                StartDialouge();
            }
        }
        if(isTalking && Input.GetKeyDown(KeyCode.E))
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
        hasTalked = true;
        dialoguePointer = 0;
    }
}
