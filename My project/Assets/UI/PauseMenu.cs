using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerInput playerInput;
   


    public bool gamePaused = false;
    public GameObject pauseMenuUI;
    private VerticalLayoutGroup layoutGroup;
    public bool optionsOpened;


    public GameObject menuScreen;
    public GameObject audioMenuScreen;
    public GameObject statsMenu;

    public GameObject[] optionButtons;


    private void Start()
    {
        layoutGroup = pauseMenuUI.GetComponentInChildren<VerticalLayoutGroup>();
        gameManager = GetComponent<GameManager>();
        playerInput = GetComponent<PlayerInput>();
    }



    void Update()
    {
        if(playerInput != null)
        {
            if (playerInput.pause == true)
            {
                pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
                gamePaused = pauseMenuUI.activeSelf;
            }
        }
       

        if (gamePaused == true)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    //Pause and resume Game
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        Cursor.visible = false;
        BackButton();

    }


    //Pause Menu Options

    public void OptionsMenu()
    {
        Debug.Log("Options Selected");
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
            layoutGroup.spacing = Mathf.Lerp(layoutGroup.spacing, 267, timeElapsed);
            timeElapsed += Time.realtimeSinceStartup - startTime;
            if (layoutGroup.spacing > -100)
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
            layoutGroup.spacing = Mathf.Lerp(layoutGroup.spacing, -150, timeElapsed);
            timeElapsed += Time.realtimeSinceStartup - startTime;

            if (layoutGroup.spacing < -40)
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

    public void OpenStatsMenu()
    {
        StartCoroutine(CloseOptionSubMenu());
        menuScreen.SetActive(false);
        statsMenu.SetActive(true);

    }


    public void BackButton()
    {
        menuScreen.SetActive(true);
        audioMenuScreen.SetActive(false);
        statsMenu.SetActive(false);
    }

    public void EndGame()
    {
        gameManager.QuitGame();
    }






   

 
}
