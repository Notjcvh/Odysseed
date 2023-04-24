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
    public float distanceToPlayer;
    public GameObject talkingIndicator;
    public GameObject[] indicators = new GameObject[2];

    public bool isTalking;
    public int dialoguePointer;
    public bool playerInTalkRange;
    public TextMeshProUGUI myDialouge;
    private void Awake()
    {
        isTalking = false;
        myDialouge = dialogue.GetComponentInChildren<TextMeshProUGUI>(); 
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
            indicators[0].SetActive(false);
            indicators[1].SetActive(true);

        }
        else
        {
            playerInTalkRange = false;
            indicators[0].SetActive(true);
            indicators[1].SetActive(false);
         
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
