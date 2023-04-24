using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPC : MonoBehaviour
{
    public string NPCName;
    public TextMeshProUGUI NameTag;
    public bool hasTalked;
    public GameObject dialogue;
    public string[] dialogueList;
    private GameObject player;
    public GameObject nextChat;
    public float talkRange;
    private float distanceToPlayer;
    private GameObject talkingIndicator;
    private bool isTalking;
    private int dialoguePointer;
    private bool playerInTalkRange;
    private TextMeshProUGUI myDialouge;
    private void Awake()
    {
        isTalking = false;
        myDialouge = dialogue.GetComponentInChildren<TextMeshProUGUI>();
        talkingIndicator = this.transform.Find("Cube").gameObject;
        hasTalked = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        talkingIndicator.SetActive(!hasTalked);
        distanceToPlayer = Mathf.Abs(Vector3.Distance(transform.position, player.transform.position));
        if(distanceToPlayer < talkRange)
        {
            playerInTalkRange = true;
        }
        else
        {
            playerInTalkRange = false;
        }
        if (Input.GetKeyDown(KeyCode.E) && playerInTalkRange && !isTalking)
        {
             StartDialouge();
        }
        if(isTalking && Input.GetKeyDown(KeyCode.E) && playerInTalkRange)
        {
            dialoguePointer++;
            if (dialoguePointer == dialogueList.Length)
            {
                EndDialouge();
            }
            myDialouge.text = dialogueList[dialoguePointer];
        }
        if(dialoguePointer < dialogueList.Length)
        {
            nextChat.SetActive(true);
        }
    }
    public void StartDialouge()
    {
        isTalking = true;
        dialogue.SetActive(isTalking);
        dialoguePointer = 0;
        NameTag.text = NPCName;
    }
    public void EndDialouge()
    {
        isTalking = false;
        hasTalked = true;
        dialogue.SetActive(isTalking);
        nextChat.SetActive(false);
        dialoguePointer = 0;
        ZeroText();
    }
    public void ZeroText()
    {
        myDialouge.text = "";
    }
}
