using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameManager gameManager;
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


    private void OnEnable()
    {
        gameManager = GetComponentInParent<GameManager>();
        canvas = GetComponent<Canvas>();
        myCamera = gameManager.mainCamera;
        canvas.worldCamera = myCamera;
        canvas.planeDistance = 1f;
        menuScreen.SetActive(true);
    }
    private void OnDisable()
    {
        BackButton();
    }

    void Update()
    {
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
        gameManager.gamePaused = false;
        activatePlayer?.Raise();
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
        gameManager.LoadLevel(sceneData.ToString());
    }
}
