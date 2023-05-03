using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEditor;


public class GameManager : MonoBehaviour
{

    [Header("Referencing")]
    public static GameManager instance;
    public SceneHandler sceneManager;
    public Camera mainCamera;
    public GameObject player;

    [Header("Game Events")]
    [SerializeField] private GameEvent initializeScene;
    [SerializeField] private GameEvent initializePlayer;
    public GameEvent getObjectsFromScene; 
    public AudioSource audioSource;
    public AudioClip audioClip;

    [Header("Player Events")]
    public PlayerEvent reachedCheckpoint;

    [Header("Scene Management")]
    public Level levelToLoad;
    public Scene scene;
    public int buildindex;
    public GameObject sceneTransition;

    [Header("UI Game Objects")]
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;
    public GameObject loadingScreenUI;
      public GameObject cursor;
    public Slider loadingSlider;
    private float sliderTarget;
    public Image loadingScreenImage;
    public Sprite[] loadingScreenSprites;

    public string currentScene;
    public Vector3 startingPosition;
    public Vector3 lastReachCheckpoint;
    public Vector3 levelPosition;
    public bool loaded = false;
    public bool hasDied = false; // might be better to have as a number 
    public bool gamePaused = false;
    public AudioMixer mixer;
    public HashSet<Vector3> hasSet = new HashSet<Vector3>();
    public List<Vector3> triggeredPoints = null; // used to convert hashset to list to get transfroms of checkpoints


 

    #region Unity Functions
    private void Awake()
    {
           // this makes sure we don't find multiple instances where we have multiple game managers in the scene 
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(gameObject);

     
    }

    private void Start()
    {
        audioSource.clip = audioClip;


        //initializeScene.Raise();
        //initializePlayer.Raise();
    }
    private void Update()
    {
        if(gamePaused == true)
        {
            Pause();
        }
        else if(gamePaused == false)
        {
            Resume();
        }

        if (Input.GetKeyDown(KeyCode.O))
            player.GetComponent<PlayerManger>().currentHealth = 0;
    }
    #endregion

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadAsycnchronously(sceneName));
    }

    public void LoadLevel(Scene calledScene)
    {
        StartCoroutine(LoadAsycnchronously(calledScene.name));
    }

    IEnumerator LoadAsycnchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);


        //choose image
        if (loadingScreenSprites?.Length != 0)
        {
            int spriteIndex = UnityEngine.Random.Range(0, loadingScreenSprites.Length);
            loadingScreenImage.sprite = loadingScreenSprites[spriteIndex];
        }

        loadingScreenUI.SetActive(true);
        loadingSlider.value = 0;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            sliderTarget = progress;
            // Additional check to exit the loop
            yield return null;

            if (operation.progress >= 0.9f && !operation.allowSceneActivation)
            {
                // If the scene loading progress reaches 90% and scene activation is not allowed,
                // force scene activation to complete the loading process and exit the loop.
                operation.allowSceneActivation = true;
            }
        }
        // Wait until the new scene is fully loaded
        yield return new WaitForEndOfFrame();

        // Get the objects form the new scene
        mainCamera = Camera.main;

        player = GameObject.FindGameObjectWithTag("Player");
        sceneManager = GameObject.FindGameObjectWithTag("Scene Handler").GetComponent<SceneHandler>();

        if(player != null)
        {
       
            SetPlayerPosition(player.transform.position);
            initializeScene?.Raise();
        }
       

        // Assign the camera to the loading screen canvas
        Canvas loadingScreenCanvas = loadingScreenUI.GetComponent<Canvas>();
        loadingScreenCanvas.worldCamera = mainCamera;
        loadingScreenCanvas.planeDistance = .15f;
        loadingScreenCanvas.sortingLayerName = "UI";
        loadingScreenCanvas.sortingOrder = 2;

        while (loadingSlider.value != sliderTarget)
        { 
            loadingSlider.value = Mathf.MoveTowards(loadingSlider.value, sliderTarget, .75f * Time.deltaTime);
            yield return null;
        }

        loadingScreenUI.SetActive(false);

        scene = SceneManager.GetActiveScene();
     
        if (scene.isLoaded && loadingScreenUI.activeSelf == false)
        {
            buildindex = scene.buildIndex;
            if(player != null)
                DisplaySceneTransitionUI(scene);
        }
    }


    public void SetPlayerPosition(Vector3 position)
    {
        if (hasSet.Count > 0)
        {
            startingPosition = lastReachCheckpoint;
        }
        else
        {
            startingPosition = position;
        }

        player.transform.position = startingPosition;
      
    }
    public void Convert()
    {
        triggeredPoints = new List<Vector3>(hasSet);
        Vector3 b = triggeredPoints[triggeredPoints.Count - 1];
        lastReachCheckpoint = b;
        reachedCheckpoint?.Raise();
    }

    //Pause and resume Game
    private void Pause()
    {
        //Debug.Log("Pause");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
       pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }


    #region Recieving Game Event Calls 
    //activate game over Ui --> Listening for playerhasDied GameEvent
    public void PlayerHasDied()
    {
        gameOverUI.SetActive(true);
       
    }
    
    //If the player dies reload the current scene by calling the event
    public void ReloadLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        LoadLevel(scene);
    }

    public void  QuitGame()
    {
       // struck the if/else for isEditor, as this makes Unity very mad if you try to build the game. - Thomas
       //if(Application.isEditor)
       //{
       //   EditorApplication.isPlaying = false;
       //}

        Application.Quit();
    }

    public void AssignCanvasValues()
    {
        // Get the objects form the new scene
        mainCamera = Camera.main;



        // Assign the camera to the loading screen canvas
        Canvas loadingScreenCanvas = loadingScreenUI.GetComponent<Canvas>();
        loadingScreenCanvas.worldCamera = mainCamera;
        loadingScreenCanvas.planeDistance = .15f;
        loadingScreenCanvas.sortingLayerName = "UI";
        loadingScreenCanvas.sortingOrder = 2;


        Canvas pauseMenuCanvas = pauseMenuUI.GetComponent<Canvas>();
        pauseMenuCanvas.worldCamera = mainCamera;
        pauseMenuCanvas.planeDistance = .15f;
        pauseMenuCanvas.sortingLayerName = "UI";
        pauseMenuCanvas.sortingOrder = 4;

    }


    public void ClearCheckpoints()
    {
        hasSet.Clear();
        triggeredPoints.Clear();
    }
    #endregion

    #region Audio Settings
    public void SetMasterVolume(float sliderValue)
    {
        mixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }
    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }
    public void SetSoundEffectVolume(float sliderValue)
    {
        mixer.SetFloat("SoundEffects", Mathf.Log10(sliderValue) * 20);
    }
    #endregion

    #region Scene Transition UI
    public void DisplaySceneTransitionUI(Scene scene)
    {
        if(sceneTransition == null || sceneManager.isADungeon == true)
        {
            initializeScene?.Raise();
            initializePlayer?.Raise();
            return;
        }
        else
        {
            sceneTransition.SetActive(true);


            foreach (Transform item in sceneTransition.transform)
            {
                ObjData data = item.GetComponent<ObjData>();
                if (data == null || scene.name != data.myData.ToString())
                {
                    continue;
                }
                else
                {
                    item.gameObject.SetActive(true);
                    StartCoroutine(ColorLerp(item.GetComponent<Image>(), data.myCurve));
                }
            }
        }
    }


    IEnumerator ColorLerp(Image image, AnimationCurve animationCurve)
    {
        float timeElapsed = 0;
        Keyframe[] keys = animationCurve.keys;
        Keyframe lastKey = keys[keys.Length - 1];
        float end = lastKey.time;
        Color startColor = image.color; // Get the starting color from the image
        Color alphaColor = startColor;
        alphaColor.a = 0;
        while (timeElapsed < end)
        {
            float t = animationCurve.Evaluate(timeElapsed); // Evaluate the animation curve at the current time
            Color lerpedColor = Color.Lerp(startColor, alphaColor, t); // Interpolate between the starting color and alphaColor
            image.color = lerpedColor; // Set the lerped color to the image'
            //Debug.Log(image.color + " this is the time" + timeElapsed);
            
            timeElapsed += Time.deltaTime;
            if (timeElapsed > end / 2)
                initializePlayer?.Raise();
            yield return null;
        }
        sceneTransition.SetActive(false);
        image.color = startColor;
    }
    #endregion
}

public enum SceneData
{
    None,
    #region BuildScenes
    Title,
    Vineyard,
    Dungeon1,
    Vineyard2,
    PotatoLands,
    Dungeon2,
    CarrotKhanate,
    Dungeon3,
    TreeOfLife,
    #endregion
    #region Testing Scenes 
    Sterling_PlayerTestScene,
    #endregion 
}


