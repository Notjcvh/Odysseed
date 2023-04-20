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

    [Header("Game Events")]
    [SerializeField] private GameEvent initializeScene;

    public AudioSource audioSource;
    public AudioClip audioClip;

    [Header("Scene Management")]
    public Level levelToLoad;
    public Scene scene;
    public int buildindex;
    public GameObject sceneTransition;
    public TextMeshProUGUI[] displayText;
    public Level[] levels;

  

    [Header("UI Game Objects")]
    public GameObject gameOverUI;
    public GameObject loadingScreenUI;
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
    }
    private void Update()
    {
      /* if(Input.GetKeyDown(KeyCode.Y))
       {
            LoadLevel(SceneManager.GetActiveScene());
       }*/


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

        while (loadingSlider.value != sliderTarget)
        { 
            loadingSlider.value = Mathf.MoveTowards(loadingSlider.value, sliderTarget, .75f * Time.deltaTime);
            yield return null;
        }

        loadingScreenUI.SetActive(false);

        scene = SceneManager.GetActiveScene();

        if (scene.isLoaded)
        {
            buildindex = scene.buildIndex;
            DisplaySceneTransitionUI(scene);
            initializeScene?.Raise();
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
        initializeScene?.Raise();
    }
    public void Convert()
    {
        triggeredPoints = new List<Vector3>(hasSet);
        Vector3 b = triggeredPoints[triggeredPoints.Count - 1];
        lastReachCheckpoint = b;
    }

    #region Recieving Game Event Calls 
    //activate game over Ui --> Listening for playerhasDied GameEvent
    public void PlayerHasDied()
    {
        if (gameOverUI.activeSelf == false)
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
        sceneTransition.SetActive(true);
        displayText = sceneTransition.GetComponentsInChildren<TextMeshProUGUI>();
        if (sceneTransition.activeInHierarchy)
        {
            // setting the pannel and TMP GUI prefab to active 
            string currentSceneName = scene.name;

            foreach (var level in levels)
            {
                if (level.sceneName.ToString() != currentSceneName)
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
