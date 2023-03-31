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
    [SerializeField] private AudioType backgroundMusic;

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
        //

    }

    private void Update()
    {
       // Debug.Log(audioJobSent);
        if (sceneTransition.activeInHierarchy == true)
        {
            playerManger.inputsEnable = false;
        }
        else
        {
            playerManger.inputsEnable = true;
           
        }


        if (source.isPlaying == false)
        {
            if (audioJobSent == false)
            {
                audioJobSent = true;
                audioController.PlayAudio(backgroundMusic, false, 0, true);
                StartCoroutine(WaitAndPlayAgain());
            }
        }
      
    }
    #endregion

    #region Sound looping

    IEnumerator WaitAndPlayAgain()
    {
        yield return new WaitForSecondsRealtime(4);
        audioJobSent = false;
    }
    #endregion

    void IntializeScene()
    {
        if (gameManager.buildindex >= 0) //the player is active after scene 1
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

            string currentSceneName = gameManager.scene.name;

            foreach (var level in levels)
            {
                if (level.sceneName != currentSceneName)
                    continue;
                else
                {
                    for (int i = 0; i < displayText.Length; i++)
                    {
                        if (i == 0)
                            displayText[i].SetText(level.levelName);
                        else
                            displayText[i].SetText(level.description);
                    }
                }
            }
            
        }
        else
            return;
    }
    #endregion

}
