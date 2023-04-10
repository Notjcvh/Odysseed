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

    [Header("Background Audio Caller")]
    public AudioType playingAudio; // the currently playing audio
    [SerializeField] private AudioType queueAudio; // the next audio to play
    private AudioSource audioSource;
    public AudioClip clip;
    public bool audioJobSent = false; // if job sent is true then it won't play
    private Dictionary<AudioType, AudioClip> ourAudio = new Dictionary<AudioType, AudioClip>();
    private List<AudioController.AudioObject> audioObjects = new List<AudioController.AudioObject>();

    #region Unity Functions
    private void Start()
    {
        // Get Components 
        player = GameObject.FindGameObjectWithTag("Player");
        playerManger = player.GetComponent<PlayerManger>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        gameManager.SetPlayerPosition(player.transform.position);
        audioController = GetComponent<AudioController>();
        audioSource = GetComponent<AudioSource>();

        IntializeScene();
    }

    private void Update()
    {
        if (audioJobSent == false )
        {
            audioJobSent = true;    
            ManageAudio(queueAudio);
            StartCoroutine(WaitToPlay(clip.length));
        }
    }
    #endregion

    #region Sound looping
    // Call this function if we want to stop the currently playing audio 
    void StopAudio(AudioType audioType) 
    {
        if(audioSource.isPlaying)
        {
            audioController.StopAudio(audioType, true, 0, false);
            audioJobSent = false;
        }
    }
    // Call this function if we want to change audio 
    void QueueAudio(AudioType audioType)
    {
        queueAudio = audioType;
    }

    void ManageAudio(AudioType type)
    {
       if(ourAudio.Count < 1)
       {
            // Loop through each audio track
            foreach (AudioController.AudioTrack track in audioController.tracks)
            {
                // Access the audio objects in each track
                audioObjects.AddRange(track.audio);
                // Loop through each audio object in the track
                foreach (AudioController.AudioObject audioObject in audioObjects)
                {
                    // this should add all our audio to the dictionary
                    ourAudio.Add(audioObject.type, audioObject.clip);
                }
            }
       }
        if (ourAudio.ContainsKey(type))
        {
            clip = ourAudio[type];
        }
        if (type != playingAudio)
        {
            audioController.PlayAudio(type, false, 0, false);
            playingAudio = type;
        }
        else
        {
            audioController.PlayAudio(playingAudio, false, 0, false);
        }
    }
    IEnumerator WaitToPlay(float time)
    {
        yield return new WaitForSecondsRealtime(time);
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

        ActivatePlayer();
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

    void ActivatePlayer()
    {
        playerManger.inputsEnable = true;
    }

    public void DeactivatePlayer()
    {
       playerManger.inputsEnable = false;
    }

}
