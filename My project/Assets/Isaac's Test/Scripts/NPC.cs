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
    private AudioController audioController;
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
    public bool audioClipPlayed = false;

    [Header("Audio Caller")]
    public AudioType queuedAudio; // the audio we will play when talking is true
    private Dictionary<AudioType, AudioClip> ourAudio = new Dictionary<AudioType, AudioClip>();
    public bool audioTableSet = false; // if job sent is true then it won't play
    private List<AudioController.AudioObject> audioObjects = new List<AudioController.AudioObject>();

    [Header("Additional Special Actions")] 
    public NPC_AdditionalActions actions;
    public PlayerEventsWithData playerEvent; //if the NPC has an objective
    public AudioClip startAudioClip;
    public AudioClip endAudioClip;


    //Getters and Setters
    public Dictionary<AudioType, AudioClip> MyAudio { get { return ourAudio; } private set { ourAudio = value; } }
    private void Awake()
    {
        isTalking = false;
        myDialouge = dialogue.GetComponentInChildren<TextMeshProUGUI>();
        indicator = talkingIndicator.GetComponent<IndicatorScript>();
        hasTalked = false;
        player = GameObject.FindGameObjectWithTag("Player");
        audioController = this.GetComponent<AudioController>();
        audioSource = GetComponent<AudioSource>();

        //Prefab has the objecitve component so it can assign objectives
        if (this.GetComponent<NPC_AdditionalActions>() != null)
            actions = this.GetComponent<NPC_AdditionalActions>();
    }
    private void Start()
    {

        if (actions == null || actions.willSpeak == false)
        {
            //choose a clip from the audio controller
            SetAudio();
        }
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
            StartCoroutine(TalkCoroutine(audioSource, startAudioClip));
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
        if (actions == null || actions.willSpeak == false)
        {
            PlayAudio();
        }
    }
    public void EndDialouge()
    {
        isTalking = false;
        hasTalked = true;
        audioClipPlayed = false;
        dialogue.SetActive(isTalking);
        dialoguePointer = 0;
        ZeroText();

        if (actions != null && actions.willSpeak == true)
            // Play the audio clip and wait for it to finish to call the Start Dialouge
            actions.Talk(audioSource, endAudioClip);
        if (actions != null && playerEvent != null && actions.willAssignObjective == true)
            actions.AssignObjective(playerEvent);
        else
        {
            audioController.StopAudio(queuedAudio, false, 0, false);
        }
    }
    public void NextDialogue()
    {
        if (actions == null || actions.willSpeak == false)
        {
            StopAudio();
            PlayAudio();
        }
        //ac.tracks[dialoguePointer].stop
        dialoguePointer++;
        //ac.tracks[dialoguePointer].PlayAudio();
    }
    public void ZeroText()
    {
        myDialouge.text = "";
    }

    void PlayAudio()
    {
        audioController.PlayAudio(queuedAudio, false, 0, false);
    }
    void StopAudio()
    {
        audioController.StopAudio(queuedAudio, false, 0, false);
    }
    private void SetAudio()
    {      
        queuedAudio = AudioType.None;
        //choose audio then assign to the audio source 
        int numberOfRandomNumbers = 1; // Number of random numbers to generate
        int minRange = 1; // Minimum value for random numbers
        int maxRange = 8;

        for (int i = 0; i < numberOfRandomNumbers; i++)
        {
            int randomNumber = Random.Range(minRange, maxRange + 1);
            switch (randomNumber)
            {
                case 1:
                    queuedAudio = AudioType.NpcDialogue1;
                    break;
                case 2:
                    queuedAudio = AudioType.NpcDialouge2;
                    break;
                case 3:
                    queuedAudio = AudioType.NpcDialouge3;
                    break;
                case 4:
                    queuedAudio = AudioType.NpcDialouge4;
                    break;
                case 5:
                    queuedAudio = AudioType.NpcDialouge5;
                    break;
                case 6:
                    queuedAudio = AudioType.NpcDialouge6;
                    break;
                case 7:
                    queuedAudio = AudioType.PotatoMurmur1;
                    break;
                case 8:
                    queuedAudio = AudioType.Murmur;
                    break;
            }
        }
    }

    
    IEnumerator TalkCoroutine(AudioSource audioSource, AudioClip audioClip)
    {
        // talk at the start of a dialouge 
        if (actions != null && actions.willSpeak == true)
        {

            // Play the audio clip and wait for it to finish to call the Start Dialouge
            actions.Talk(audioSource, audioClip);
            yield return new WaitUntil(() => !audioSource.isPlaying);
        }

        // Start the dialogue or end Dialouge 
        StartDialouge();
    }



}



