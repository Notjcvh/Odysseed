using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;

public class GameManager : MonoBehaviour
{

    [Header("Referencing")]
    public static GameManager instance;
    public List<GameObject> allObjects = new List<GameObject>(); // all objects we want the game manager to keep

    public Vector3 startingPosition;
    public Vector3 lastReachCheckpoint;
    public Vector3 levelPosition;

    public SceneManager currentScene;
    public int buildindex;


    public bool loaded = false;
    public bool hasDied = false; // might be better to have as a number 
    public bool Levelcompleted = false;

    
    
    public bool playerHasDied;

    public Scene scene;


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

        scene = SceneManager.GetActiveScene();
        if(scene.isLoaded)
        {
            Debug.Log("Scene Loaded");
        }         
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            ReloadScene();
        }
    }
    #endregion


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




    //If the player runs into Scene trasition collider load that scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        buildindex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("This is the scene name : " + sceneName + " this is the build index : " + buildindex);
    }


    //If the player dies reload the current scene 
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // look inside the player manager start function
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
            
    }


    public void PlayerHasDied() // called in Player Manager 
    {
        hasDied = true;
        ReloadScene();
    }
    public void Convert()
    {
        
        triggeredPoints = new List<Vector3>(hasSet);
        Vector3 b = triggeredPoints[triggeredPoints.Count - 1];
        lastReachCheckpoint = b;
    }



    public void  QuitGame()
    {
        if(Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            // Play Something first then quit
            Application.Quit();
        }
    }

  
}
