using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    [Header("Referencing")]

    private static GameManager instance;
    
    public GameObject player;
    private PlayerMovement playerMovement;


    public Vector3 position;
    public Vector3 lastCheckPointPos;

    private GameObject BoneYard; // the boneyard is basically the place where we move the enemies then destory them --> this is a test right now though
    private Vector3 positionOfBoneyard;


    public VectorValue level;
    public SceneManager currentScene;


    [Header("Scene Transitions")]
    private GameObject sceneTransition;
    public float textDisappearTimer = 1.3f;
    public float countdown;
    private bool sceneTransitonTextActive = false;
    public TextMeshProUGUI displayText;


    public bool hasDied = false; // might be better to have as a number 

    public HashSet<Vector3> hasSet = new HashSet<Vector3>();
    public List<Vector3> triggeredPoints = null; // used to convert hashset to list to get transfroms of checkpoints
    private GameObject[] checkpoints = null;
    
    public bool loaded = false;

    #region Unity Functions
    private void Awake()
    {
        if (instance == null) // this makes sure we don't find multiple instances where we have multiple game managers in the scene 
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(gameObject);

        // Get Components 
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        //Instantiate
        positionOfBoneyard = level.boneYard;
        BoneYard = Instantiate(GameAssets.i.BoneYard, positionOfBoneyard, Quaternion.identity); // creates the boneyard based on Vector 3 saved in the Dungeon 1 Scriptable Object
        sceneTransition = Instantiate(GameAssets.i.SceneTransitionCanvas);
        
        //Start Scene Transitions 
        sceneTransitonTextActive = true;
        countdown = textDisappearTimer;
        DisplayText(sceneTransition);

        //Find Checkpoints 
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoints"); // for disabling all the checkpoint meshes
        foreach (var item in checkpoints)
        {
            item.GetComponent<MeshRenderer>().enabled = false;
        }

        ReloadPosition();

    }
    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (sceneTransitonTextActive == true)
        {
            playerMovement.stopMovementEvent = true;
            //  StartCoroutine(Transitioning());
            if (countdown >= 0)
            {
                
                countdown -= Time.deltaTime;
                loaded = true;
            }
            if (countdown <= 0)
            {
                sceneTransitonTextActive = false;
                loaded = false;
                playerMovement.stopMovementEvent = false;
                countdown = textDisappearTimer;
               
            }
        }


        //Debuging 

        if(Input.GetKeyDown(KeyCode.I))
        {
            checkpoints = GameObject.FindGameObjectsWithTag("Checkpoints"); // for disabling all the checkpoint meshes
            foreach (var item in checkpoints)
            {
                item.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            ReloadPosition();
            PlayerHasDied();
 

        }



    }


    #endregion

    #region Scene Transitions 
    public void DisplayText(GameObject scene)
    {
        if (sceneTransition != null)
        {
            foreach (Transform t in scene.transform)
               t.gameObject.SetActive(true); // setting the pannel and TMP GUI prefab to active 
            
            displayText = scene.GetComponentInChildren<TextMeshProUGUI>(); 
            displayText.SetText(level.levelName);
        }
        else
            return;       
    }
    #endregion



    public void PlayerHasDied() // called in Player Manager 
    {
        hasDied = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // look inside the player manager start function    
    }




    public void Convert()
    {
        triggeredPoints = new List<Vector3>(hasSet);

    }
    public void ReloadPosition()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (hasSet.Count > 0)
        {
            Vector3 b = triggeredPoints[triggeredPoints.Count - 1];
            Debug.Log(b);
            lastCheckPointPos = b;
            player.transform.position = lastCheckPointPos;
        }
        else
        {
            player.transform.position = level.initialStartValue;
            position = player.transform.position;
        }
    }
  



}
