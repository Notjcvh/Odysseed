using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject pauseMenuUI;
    public  VerticalLayoutGroup layoutGroup;
    public bool optionsOpened;

    public GameObject menuScreen;
    public GameObject audioMenuScreen;
    public GameObject controlScreen;
    public GameObject quitScreen;

    public GameObject[] optionButtons;

    public SceneEvent activatePlayer;
    public SceneEvent deactivatePlayer;


    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.gamePaused == true)
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
        //Debug.Log("Pause");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;




        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Resume()
    {
        gameManager.gamePaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        BackButton();
    }

    public void PlayAudio()
    {
        gameManager.audioSource.Play();
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
        Resume();
        activatePlayer?.Raise();
    }

    public void BackButton()
    {
        menuScreen.SetActive(true);
        audioMenuScreen.SetActive(false);
        controlScreen.SetActive(false);
        quitScreen.SetActive(false);

    }

    public void EndGame()
    {
        gameManager.QuitGame();
        Debug.Log("Game Quit");
    }






   

 
}
