using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject pauseMenuUI;
    private VerticalLayoutGroup layoutGroup;
    public float lerpduration = 3;
   public bool optionsOpened;


    public GameObject menuScreen;
    public GameObject audioMenuScreen;

    public GameObject audioButton;


    private void Start()
    {
        layoutGroup = pauseMenuUI.GetComponentInChildren<VerticalLayoutGroup>();
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        }
    }

    //Pause Menu Options

    public void OptionsMenu()
    {
        Debug.Log("Options Selected");
        //Start Couritine loop through
      
        if(optionsOpened != true)
        {
            StartCoroutine(OpenOptionSubMenu());
            optionsOpened = true;
        }
        else
        {
            Debug.Log("Options closed");
            StartCoroutine(CloseOptionSubMenu());
            optionsOpened = false;
        }
    }

    IEnumerator OpenOptionSubMenu()
    {
        float timeElapsed = 0;
        while(timeElapsed < lerpduration)
        {
            layoutGroup.spacing = Mathf.Lerp(layoutGroup.spacing, -25, timeElapsed / lerpduration);
            timeElapsed += Time.deltaTime;
            if(layoutGroup.spacing > -100)
                audioButton.SetActive(true);
            yield return null;
        }
    }

    IEnumerator CloseOptionSubMenu()
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpduration)
        {
            layoutGroup.spacing = Mathf.Lerp(layoutGroup.spacing, -150, timeElapsed / lerpduration);
            timeElapsed += Time.deltaTime;

            if (layoutGroup.spacing < -40)
                audioButton.SetActive(false);
            yield return null;
        }
    }


    public void OpenAudioMenu()
    {
        menuScreen.SetActive(false);
        audioMenuScreen.SetActive(true);
    }





    /*  public void Resume()
      {
          pauseMenuUI.SetActive(false);
          Time.timeScale = 1f;
          GamePaused = false;
          Cursor.visible = false;

      }

      void Pause()
      {
          pauseMenuUI.SetActive(true);
          Time.timeScale = 0f;
          GamePaused = true;
          Cursor.visible = true;
      }*/
}
