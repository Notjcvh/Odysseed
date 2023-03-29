using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneHandeler : MonoBehaviour
{
    [Header("Referencing")]
    [SerializeField] private GameManager gameManager; 
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerManger playerManger;
    [SerializeField] private AudioController audioController;

    [Header("Scene Transitions")]
    public GameObject sceneTransition;
    public TextMeshProUGUI[] displayText;
    public Level[] levels;

    public Vector3 spawnPosition;


    public AudioSource source;
    public bool audioJobSent = false; 

    #region Unity Functions
    private void Start()
    {
        // Get Components 
        player = GameObject.FindGameObjectWithTag("Player");
        playerManger = player.GetComponent<PlayerManger>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        gameManager.SetPlayerPosition(player.transform.position);
        audioController = GetComponent<AudioController>();

        IntializeScene();
        audioController.RestartAudio(AudioType.PlayerAttack, false, 0, true);
    }

    private void Update()
    {
        if (sceneTransition.activeInHierarchy == true)
        {
            playerManger.inputsEnable = false;
        }
        else
        {
            playerManger.inputsEnable = true;
           
        }

        if (audioController.source != null)
        {
            audioController.PlayAudio(AudioType.PlayerAttack, false, 0, false);
         

            if (audioController.source.isPlaying == true)
            {
                Debug.Log("Playign Audio");
                audioJobSent = true;
            }
            else
            {
                Debug.Log("Not playing audio");

                audioJobSent = false;
            }


        }
    }
    #endregion

    void IntializeScene()
    {
        if (gameManager.buildindex > 1) //the player is active after scene 1
        {
            sceneTransition.SetActive(true);
            DisplaySceneTransitionUI(sceneTransition);
            player = GameObject.FindGameObjectWithTag("Player");
        }

        //Load the player position
        spawnPosition = gameManager.startingPosition;
        player.transform.position = spawnPosition;
    }

    #region Scene Transition UI
    public void DisplaySceneTransitionUI(GameObject scene)
    {
        if (sceneTransition.activeInHierarchy)
        {
            // setting the pannel and TMP GUI prefab to active 
            displayText = scene.GetComponentsInChildren<TextMeshProUGUI>();

            for (int i = 0; i < displayText.Length; i++)
            {
                if (i == 0)
                    displayText[i].SetText(levels[gameManager.buildindex].levelName);
                else
                    displayText[i].SetText(levels[gameManager.buildindex].description);
            }
        }
        else
            return;
    }
    #endregion

}
