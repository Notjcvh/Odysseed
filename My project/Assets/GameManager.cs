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
    public GameObject sceneTransition;
    public GameObject player;
    private PlayerMovement playerMovement;

    public VectorValue room;
    public SceneManager currentScene;
    public TextMeshProUGUI displayText;

    public float textDisappearTimer = 1.3f;
    public float countdown;
    private bool sceneTransitonTextActive = false;

    #region Unity Functions
    private void Awake()
    {
       sceneTransition = Instantiate(GameAssets.i.SceneTransitionCanvas);
       playerMovement = player.GetComponent<PlayerMovement>();
       sceneTransitonTextActive = true;
        
        countdown = textDisappearTimer;
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
    #endregion



}
