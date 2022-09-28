using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    [SerializeField] private Transform player;
    private PlayerController playerController;
    private NavMeshAgent navMeshAge;
    public GameObject Dialogue;

    public float distanceFromPlayer;
    public float talkRange;
    public bool isFollowing;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Dialogue.SetActive(false);
        playerController = player.GetComponent<PlayerController>();
    }
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);

        if (Input.GetKeyDown(KeyCode.E) && distanceFromPlayer < talkRange)
        {
            StartDialouge();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EndDialouge();
        }
    }
    public void StartDialouge()
    {
        playerController.isTalking = true;
        Dialogue.SetActive(true);
    }
    public void EndDialouge()
    {
        playerController.isTalking = false;
        Dialogue.SetActive(false);
    }


}
