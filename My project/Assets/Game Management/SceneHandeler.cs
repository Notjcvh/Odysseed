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

    public bool sceneActivated = false;

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
    }

    private void Update()
    {
        if(sceneActivated == true)
        {
           if (audioJobSent == false)
           {
                audioJobSent = true;
                ManageAudio(queueAudio);
                //StartCoroutine(WaitToPlay(clip.length));
           }
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            StopAudio();
        }

        if (gameManager.gamePaused == true)
        {
            DeactivatePlayer();
        }
        else
        {
            ActivatePlayer();
        }

    }
    #endregion

    #region Sound looping
    // Call this function if we want to stop the currently playing audio 
     public void StopAudio() 
    {
        StopCoroutine(WaitToPlay(clip.length));
        if(audioSource.isPlaying)
        {
            audioController.StopAudio(playingAudio, false, 0, false);
            StartCoroutine(WaitToPlay(3));
        }
    }
    // Call this function if we want to change audio 
   public void QueueAudio(SceneEvent sceneEvent)
   {
        switch (sceneEvent.name)
        {
            case ("Audio_DungeonOne"):
                queueAudio = AudioType.DungeonOne;
                break;
            case ("Audio_RotBoss"):
                queueAudio = AudioType.RotBoss;
                break;
            case ("Audio_MainMenu"):
                queueAudio = AudioType.MainMenu;
                break;
            default:
                break;
        }
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

    public void IntializeScene()
    {

        sceneActivated = true;
       // if (gameManager.buildindex >= 0) //the player is active after scene 1
       // {
            player = GameObject.FindGameObjectWithTag("Player");
      //  }

        
        //Load the player position
        spawnPosition = gameManager.startingPosition;
        player.transform.position = spawnPosition;


        playerManger.CreateHealthBar();
        ActivatePlayer();
    }

   


    void ActivatePlayer()
    {
        playerManger.inputsEnable = true;
       
    }

    public void DeactivatePlayer()
    {
       playerManger.inputsEnable = false;
    }

}
