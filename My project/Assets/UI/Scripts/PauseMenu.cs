using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameManager gameManager;
    public SceneHandler sceneHandler;
    private SceneData sceneData = SceneData.Title;
    public Camera myCamera;
  
    public Canvas canvas;
    public  VerticalLayoutGroup layoutGroup;
    public bool optionsOpened;

    public GameObject menuScreen;
    public GameObject audioMenuScreen;
    public GameObject controlScreen;
    public GameObject quitScreen;

    public GameObject[] optionButtons;

    public SceneEvent activatePlayer;
    public SceneEvent deactivatePlayer;
    public PlayerEvent callObjective;


    private void OnEnable()
    {
        gameManager = GetComponentInParent<GameManager>();
        sceneHandler = gameManager.sceneHandler;
        canvas = GetComponent<Canvas>();
        myCamera = Camera.main;
        canvas.worldCamera = myCamera;
        canvas.planeDistance = .15f;
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 4;
        menuScreen.SetActive(true);
        gameManager.cursor.SetActive(true);
    }
    private void OnDisable()
    {
        BackButton();
        gameManager.cursor.SetActive(false);
    }

    void Update()
    {
        if(gameManager.mainCamera != null)
            transform.LookAt(gameManager.mainCamera.transform);
    }


    public void PlayAudio()
    {
        gameManager.audioSource.Play();
    }


    //Pause Menu Options
    public void OptionsMenu()
    {
         //Start Couritine loop through
        if (optionsOpened == true)
        {
            StartCoroutine(CloseOptionSubMenu());
        }
        else
        {
            StartCoroutine(OpenOptionSubMenu());
        }
    }

    IEnumerator OpenOptionSubMenu()
    {
        optionsOpened = true;
        float startTime = Time.realtimeSinceStartup;
        float timeElapsed = 0;
        while (timeElapsed < 1)
        {
            layoutGroup.spacing = Mathf.Lerp(layoutGroup.spacing, 80, timeElapsed);
            timeElapsed += Time.realtimeSinceStartup - startTime;
            if (layoutGroup.spacing > -20)
                foreach (var item in optionButtons)
                {
                    item.SetActive(true);
                }
            yield return null;
        }
    }

    IEnumerator CloseOptionSubMenu()
    {
        optionsOpened = false;
        float timeElapsed = 0;
        float startTime = Time.realtimeSinceStartup;
        while (timeElapsed < 1)
        {
            layoutGroup.spacing = Mathf.Lerp(layoutGroup.spacing, -170, timeElapsed);
            timeElapsed += Time.realtimeSinceStartup - startTime;

            if (layoutGroup.spacing < -20)
                foreach (var item in optionButtons)
                {
                    item.SetActive(false);
                }
            yield return null;
        }
    }


    public void OpenAudioMenu()
    {
        menuScreen.SetActive(false);
        audioMenuScreen.SetActive(true);
        StartCoroutine(CloseOptionSubMenu());
    }

    public void OpenControlMenu()
    {
        StartCoroutine(CloseOptionSubMenu());
        menuScreen.SetActive(false);
        controlScreen.SetActive(true);
        quitScreen.SetActive(false);
    }

    public void OpenQuitMenu()
    {
        menuScreen.SetActive(false);
        controlScreen.SetActive(false);
        quitScreen.SetActive(true);
    }

    public void Continue()
    {
        gameManager.gamePaused = !gameManager.gamePaused;
       // activatePlayer?.Raise();
        callObjective?.Raise();
    }

    public void BackButton()
    {
        menuScreen.SetActive(true);
        audioMenuScreen.SetActive(false);
        controlScreen.SetActive(false);
        quitScreen.SetActive(false);

    }




    public void QuitToMenu()
    {
        BackButton();
        gameManager.gamePaused = false;
        gameManager.BackUpClose();
        gameManager.hasSet.Clear(); //Clear all checkpoints 
        gameManager.LoadLevel(sceneData.ToString());
    }
}
