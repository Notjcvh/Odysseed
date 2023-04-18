using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;




public class GameOverUI : MonoBehaviour
{
    public GameManager gameManager;
    public GameEvent restart;
    public GameEvent quit;
 
    public VideoPlayer videoPlayer;
    public GameObject[] uiElements; // activate each object in the array
    public TextMeshProUGUI instructionText; // what instructions do we display to the player 

    public bool gameOverIncremented = false; // to check if the game over has started

    [SerializeField] private Button continueButton;
    [SerializeField] private Button endButton;


    [TextAreaAttribute] public string continueButtonText;
    [TextAreaAttribute] public string endButtonText;



    private void OnEnable()
    {
        ButtonHoverChecker.OnButtonHover += HandleButtonHover;
    }

    private void OnDisable()
    {
        ButtonHoverChecker.OnButtonHover -= HandleButtonHover;
    }


   /* public void PlayAudio()
    {
        gameManager.audioSource.Play();
    }*/

    void PlayVideo()
    {
        videoPlayer.Play();
        StartCoroutine(ActivateUi((float)videoPlayer.length));
    }

    IEnumerator ActivateUi(float time)
    {
        yield return new WaitForSeconds(time/2);
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetActive(true);
        }
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void HandleButtonHover(GameObject obj, bool isHovering)
    {
        if(isHovering)
        {
           
            if(obj == continueButton.gameObject)
            {
                instructionText.text = continueButtonText;
            }
            else
            {
                instructionText.text = endButtonText;
            }
        }
        else
        {
            instructionText.text = "";
        }
    }


   public void Restart()
   {
        CloseUi();
        restart.Raise();
        
   }

   public void Quit()
   {
        CloseUi();
        quit.Raise();
   }


    void CloseUi()
    {
        this.gameObject.SetActive(false);
        Cursor.visible = false;
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetActive(false);
        }
    }
}
