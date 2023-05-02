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

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip startAudioClip;
    public AudioClip endAudioClip;
    
    [Header("Additional Actions")]
    public NPC_AdditionalActions actions;
    public PlayerEventsWithData playerEvent; //if the NPC has an objective

   
    private void Awake()
    {
        isTalking = false;
        myDialouge = dialogue.GetComponentInChildren<TextMeshProUGUI>();
        indicator = talkingIndicator.GetComponent<IndicatorScript>();
        hasTalked = false;
        player = GameObject.FindGameObjectWithTag("Player");
        ac = this.GetComponent<AudioController>();
        audioSource = GetComponent<AudioSource>();

        //Prefab has the objecitve component so it can assign objectives
        if (this.GetComponent<NPC_AdditionalActions>() != null)
            actions = this.GetComponent<NPC_AdditionalActions>();
    }
    void Update()
    {
       
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
            // talk at the start of a dialouge 
            if (actions != null && actions.willSpeak == true)
                actions.Talk(audioSource, startAudioClip);

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
        // Talk at the end of the dialouge 




        //assign objective
        if (actions != null && playerEvent != null && hasTalked == false)
            actions.AssignObjective(playerEvent);

        isTalking = false;
        hasTalked = true;
        dialogue.SetActive(isTalking);
        dialoguePointer = 0;
        ZeroText();

      
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



