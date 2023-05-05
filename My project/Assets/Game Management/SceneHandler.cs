using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    [Header("Referencing")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject player;
    public bool isADungeon;

    [SerializeField] private PlayerManger playerManger;
    [SerializeField] private AudioController audioController;
    private PlayerInput playerInput;
    public InteractionStates sceneStates;

    public bool sceneActivated = false;
    public Vector3 spawnPosition;

    public bool tutorialActivated = false;
    public bool countinuebutton;

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
        //  gameManager.SetPlayerPosition(player.transform.position);
        audioController = GetComponent<AudioController>();
        audioSource = GetComponent<AudioSource>();
        SetAudio();
    }

    private void Update()
    {
        Debug.Log(sceneStates);
        if (sceneActivated == true)
        {
            if (audioJobSent == false)
            {
                audioJobSent = true;
                ManageAudio(queueAudio);
                StartCoroutine(WaitToPlay(clip.length));
            }
        }
        if (playerInput.pause || countinuebutton == true)
        {
            gameManager.gamePaused = (!gameManager.gamePaused);
            switch (gameManager.gamePaused, playerManger.isTalking, tutorialActivated)
            {
                case (false, false, false):
                    SetState(InteractionStates.Active);
                    break;
                case (true, false, false):
                    SetState(InteractionStates.Passive);
                    break;
                case (true, true, false): // game is paused but we are still talking 
                case (false, true, false): //game is not puased, we are talking  
                case (true, false, true): // game is paused but we are still in a tutorial 
                case (false, false, true): // game is not paused, wa are is dialouge
                    SetState(InteractionStates.Passive);
                    break;
            }

            if (countinuebutton == true)
                countinuebutton = false;
        }
    }
    #endregion



    public void SetState(InteractionStates newState)
    {
        if (newState != sceneStates)
        {
            //On Leave from previous State
            switch (sceneStates)
            {
                case InteractionStates.Passive:
                    playerInput.ResetPassiveInputs();
                    break;
                case InteractionStates.Active:
                    playerInput.ResetActiveInputs();
                    break;
            }
            sceneStates = newState;
            //On Enter
            switch (sceneStates)
            {
                case InteractionStates.Passive:
                    DeactivatePlayer();
                    break;
                case InteractionStates.Active:
                    ActivatePlayer();
                    break;
            }
        }
    }

    #region Sound looping
    // Call this function if we want to stop the currently playing audio 
    public void StopAudio(float delay)
    {
        StopCoroutine(WaitToPlay(clip.length));
        if (audioSource.isPlaying)
        {
            audioController.StopAudio(playingAudio, false, 0, false);
            StartCoroutine(WaitToPlay(delay));
        }
    }
    // Call this function if we want to change audio 
    public void QueueAudio(SceneEvent sceneEvent)
    {
        Debug.Log(sceneEvent.name);
        switch (sceneEvent.name)
        {

            case ("Audio_Dungeon1"):
                queueAudio = AudioType.DungeonOne;
                break;
            case ("Audio_Dungeon2"):
                queueAudio = AudioType.DungeonTheme_2;
                break;
            case ("Audio_PrisonTheme"):
                queueAudio = AudioType.PrisonTheme;
                break;
            case ("Audio_RotBoss"):
                queueAudio = AudioType.RotBoss;
                break;
            case ("Audio_PotatoKingBoss"):
                queueAudio = AudioType.PotatoKingMusic;
                break;
            case ("Audio_CastleTheme"):
                queueAudio = AudioType.CastleTheme;
                break;
            case ("Audio_CarrotLands"):
                queueAudio = AudioType.KarrotLands;
                break;
            case ("Audio_CarrotKahnPhase1"):
                queueAudio = AudioType.CK_PhaseOne_Music;
                break;
            case ("Audio_CarrotKahnPhase2"):
                queueAudio = AudioType.CK_PhaseTwo_Music;
                break;
            default:
                break;
        }
    }

    void SetAudio()
    {
        if (ourAudio.Count < 1)
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
    }

    void ManageAudio(AudioType type)
    {

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
        player = GameObject.FindGameObjectWithTag("Player");
        playerManger = player.GetComponent<PlayerManger>();
        playerManger.CallPlayerUi();
    }

    public void InitializePlayer()
    {
        SetState(InteractionStates.Active);
        ActivatePlayer();
    }


    public void ActivatePlayer()
    {
        if (playerManger != null)
        {
            playerManger.activeInputsEnabled = true;
            playerManger.inactiveInputsEnabled = false;
            playerManger.StopMovement = false;
        }
    }

    public void DeactivatePlayer()
    {
        playerManger.playerBody.velocity = Vector3.zero;
        playerManger.activeInputsEnabled = false;
        playerManger.inactiveInputsEnabled = true;
        playerManger.SetSubState(SubStates.Idle);
        playerManger.StopMovement = false;

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
