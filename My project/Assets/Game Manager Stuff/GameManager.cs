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
    public Vector3 lastCheckPointPos;
    public GameObject sceneTransition;
    public GameObject player;
    private PlayerMovement playerMovement;

    private GameObject BoneYard; // the boneyard is basically the place where we move the enemies then destory them --> this is a test right now though
    private Vector3 positionOfBoneyard;


    public VectorValue room;
    public SceneManager currentScene;
    public TextMeshProUGUI displayText;

    public float textDisappearTimer = 1.3f;
    public float countdown;
    private bool sceneTransitonTextActive = false;
    public bool hasDied = false; // might be better to have as a number 

    public List<Transform> checkpoints;

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

       sceneTransition = Instantiate(GameAssets.i.SceneTransitionCanvas);
       playerMovement = player.GetComponent<PlayerMovement>();
       sceneTransitonTextActive = true;
        
        countdown = textDisappearTimer;
        positionOfBoneyard = room.boneYard;
        BoneYard = Instantiate(GameAssets.i.BoneYard,positionOfBoneyard,Quaternion.identity); // creates the boneyard based on Vector 3 saved in the Dungeon 1 Scriptable Object
        DisplayText(sceneTransition);
    }

    private void Update()
    { 
        if(sceneTransitonTextActive == true)
        {
            playerMovement.stopMovementEvent = true;
            //  StartCoroutine(Transitioning());
            if (countdown >= 0)
            {
                
                countdown -= Time.deltaTime;
            }
            if (countdown <= 0)
            {
                sceneTransitonTextActive = false;
                playerMovement.stopMovementEvent = false;
                countdown = textDisappearTimer;
               
            }
        }   
    }

 
    #endregion

    #region Public Functions
    public void DisplayText(GameObject scene)
    {
        if (sceneTransition != null)
        {
            foreach (Transform t in scene.transform)
               t.gameObject.SetActive(true); // setting the pannel and TMP GUI prefab to active 
            
            displayText = scene.GetComponentInChildren<TextMeshProUGUI>(); 
            displayText.SetText(room.levelName);
        }
        else
            return;       
    }

    public void PlayerHasDied()
    {
        hasDied = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // look inside the player manager start function
        hasDied = false;
    }




    #endregion



}
