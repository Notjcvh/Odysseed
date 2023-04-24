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
    public GameObject player;
    public float talkRange;
    public float distanceToPlayer;
    public GameObject talkingIndicator;
    public bool isTalking;
    public int dialoguePointer;
    public bool playerInTalkRange;
    public TextMeshProUGUI myDialouge;
    public GameObject[] indicators = new GameObject[2];
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
        ZeroText();
    }
    public void ZeroText()
    {
        myDialouge.text = "";
    }
}
