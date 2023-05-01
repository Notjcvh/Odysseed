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
    public string NPCName;
    private AudioController ac;
    public GameObject player;
    public float talkRange;
    public float distanceToPlayer;
    public GameObject talkingIndicator;
    private IndicatorScript indicator;
    public GameObject nextDialouge;
    public bool isTalking;
    public int dialoguePointer;
    public bool playerInTalkRange;
    public TextMeshProUGUI myDialouge;
    public TextMeshProUGUI NPCNameTag;
    public GameObject[] indicators = new GameObject[2];

    public NPC_AssignObjective objective;
    private void Awake()
    {
        isTalking = false;
        myDialouge = dialogue.GetComponentInChildren<TextMeshProUGUI>();
        indicator = talkingIndicator.GetComponent<IndicatorScript>();
        hasTalked = false;
        player = GameObject.FindGameObjectWithTag("Player");
        ac = this.GetComponent<AudioController>();

        //Prefab has the objecitve component so it can assign objectives
        if (this.GetComponent<NPC_AssignObjective>() != null)
            objective = this.GetComponent<NPC_AssignObjective>();
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
            indicator.changeScale = false; // stop animation 

        }
        else
        {
            playerInTalkRange = false;
            indicators[0].SetActive(true);
            indicators[1].SetActive(false);
            indicator.changeScale = true; // start animation
        }
        if (Input.GetKeyDown(KeyCode.E) && playerInTalkRange && !isTalking)
        {
             StartDialouge();
        }
        if(isTalking && Input.GetKeyDown(KeyCode.E) && playerInTalkRange)
        {
            NextDialogue();
            if (dialoguePointer == dialogueList.Length)
            {
                EndDialouge();
            }
            myDialouge.text = dialogueList[dialoguePointer];
        }
        if (dialoguePointer == dialogueList.Length - 1)
        {
            nextDialouge.SetActive(false);
        }
    }
    public void StartDialouge()
    {
        isTalking = true;
        NPCNameTag.text = NPCName;
        dialogue.SetActive(isTalking);
        nextDialouge.SetActive(true);
        dialoguePointer = 0;
    }
    public void EndDialouge()
    {
        isTalking = false;
        hasTalked = true;
        dialogue.SetActive(isTalking);
        dialoguePointer = 0;
        ZeroText();

        //assign objective
        if (objective != null)
            objective.GiveObjective();
    }
    public void NextDialogue()
    {
        //ac.tracks[dialoguePointer].stop
        dialoguePointer++;
        //ac.tracks[dialoguePointer].PlayAudio();
    }
    public void ZeroText()
    {
        myDialouge.text = "";
    }
}



