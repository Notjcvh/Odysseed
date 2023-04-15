using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPC : MonoBehaviour
{
    public bool hasTalked;
    public GameObject dialogue;
    public string[] dialogueList;
    public LayerMask whatIsPlayer;
    public float talkRange;

    public GameObject talkingIndicator;
    public bool isTalking;
    public int dialoguePointer;
    public bool playerInTalkRange;
    public TextMeshProUGUI myDialouge;
    private void Awake()
    {
        isTalking = false;
        myDialouge = dialogue.GetComponentInChildren<TextMeshProUGUI>();
        talkingIndicator = GameObject.FindGameObjectWithTag("TalkingIndicator");
        hasTalked = false;
    }
    void Update()
    {
        talkingIndicator.SetActive(!hasTalked);
        playerInTalkRange = Physics.CheckSphere(transform.position, talkRange, whatIsPlayer);
        if (Input.GetKeyDown(KeyCode.E) && playerInTalkRange && !isTalking)
        {
             StartDialouge();
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
        dialogue.SetActive(isTalking);
        dialoguePointer = 0;
    }
    public void EndDialouge()
    {
        isTalking = false;
        hasTalked = true;
        dialogue.SetActive(isTalking);
        dialoguePointer = 0;
    }
}
