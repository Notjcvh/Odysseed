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
    private PlayerInput playerInput;
    public InteractionStates sceneStates;

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
        playerInput = player.GetComponent<PlayerInput>();
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
                StartCoroutine(WaitToPlay(clip.length));
           }
        }

        if (playerInput.pause)
        {
            gameManager.gamePaused = (!gameManager.gamePaused);

            switch(sceneStates, gameManager.gamePaused)
            {
                case (InteractionStates.Active, true):
                    DeactivatePlayer();
               //     Debug.Log("Deactivate Player");
                    break;
                case (InteractionStates.Active, false):
                    ActivatePlayer();
             //       Debug.Log("Activate Player");
                    break;
            }
        }

        if(playerManger.inactiveInputsEnabled == true)
        {

        }
        

        SetState(sceneStates);
    }
    #endregion



    public void SetState(InteractionStates newState)
    {
        if (newState != sceneStates)
        {
            /*//On Leave from previous State
            switch (sceneStates)
            {
               
            }*/
            sceneStates = newState;
            //On Enter
          /*  switch (sceneStates)
            {
                
            }*/
        }
    }

    #region Sound looping
    // Call this function if we want to stop the currently playing audio 
    public void StopAudio(float delay) 
    {
        StopCoroutine(WaitToPlay(clip.length));
        if(audioSource.isPlaying)
        {
            audioController.StopAudio(playingAudio, false, 0, false);
            StartCoroutine(WaitToPlay(delay));
        }
    }
    // Call this function if we want to change audio 
   public void QueueAudio(SceneEvent sceneEvent)
   {
        switch (sceneEvent.name)
        {
            case ("Audio_RotBoss"):
                queueAudio = AudioType.RotBoss;
                break;
            case ("Audio_PotatoKingBoss"):
                queueAudio = AudioType.PotatoKingMusic;
                break;
            case ("Audio_Dungeon1"):
                queueAudio = AudioType.DungeonOne;
                break;
            case ("Audio_Dungeon2"):
                queueAudio = AudioType.DungeonTheme_2;
                break;
            case ("Audio_PrisonTheme"):
                queueAudio = AudioType.PrisonTheme;
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
        SetState(InteractionStates.Active);
        player = GameObject.FindGameObjectWithTag("Player");
        //Load the player position
     
        
        spawnPosition = gameManager.startingPosition;
        player.transform.position = spawnPosition;   // this makes the player restart 
        
        
        playerManger.CallPlayerUi();
        ActivatePlayer();
    }

    public void ActivatePlayer()
    {
        playerManger.activeInputsEnabled = true;
        playerManger.inactiveInputsEnabled = false;
        playerManger.StopMovement = false;
    //    gameManager.Cursor.SetActive(false);
    }

    public void DeactivatePlayer()
    {
       playerManger.activeInputsEnabled = false;
       playerManger.inactiveInputsEnabled = true;
       playerManger.StopMovement = true;
       playerManger.SetSubState(SubStates.Idle);
      // gameManager.Cursor.SetActive(true);
    }
}


public enum InteractionStates
{
    None, 
    Active, // player Inputs are fully functoinal 
    Passive, // does not mean the game is pause but means certain player actions are disabled
    Restricted, // Player cannot interact with the game at all
}
