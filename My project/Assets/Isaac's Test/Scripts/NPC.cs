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
        playerController.stopMovement = true;
        //playerController.isTalking = isTalking;
        if (Input.GetKeyDown(KeyCode.E) && distanceFromPlayer < talkRange)
        {
            if (!isTalking)
            {
                StartDialouge();
            }
            else
            {
                dialoguePointer++;
                if (dialoguePointer == dialogueList.Length)
                {
                    EndDialouge();
                }
                myDialouge.text = dialogueList[dialoguePointer];
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EndDialouge();
        }
    }
    public void StartDialouge()
    {
        isTalking = true;
    }
    public void EndDialouge()
    {
        isTalking = false;
        dialoguePointer = 0;
    }


}
