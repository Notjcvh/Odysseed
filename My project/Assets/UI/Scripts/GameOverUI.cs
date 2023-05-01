using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;




public class GameOverUI : MonoBehaviour
{
    public GameManager gameManager;
    public Camera myCamera;
    public Canvas canvas;
    public GameEvent restart;
    public GameEvent quit;
 
    public VideoPlayer videoPlayer;
    public Transform uiElementsHolder; // activate each object in the array
    public TextMeshProUGUI instructionText; // what instructions do we display to the player 

    [SerializeField] private Button continueButton;
    [SerializeField] private Button endButton;


    [TextAreaAttribute] public string continueButtonText;
    [TextAreaAttribute] public string endButtonText;

   

    private void OnEnable()
    {
        gameManager = GetComponentInParent<GameManager>();
        canvas = GetComponent<Canvas>();
        myCamera = gameManager.mainCamera;
        ButtonHoverChecker.OnButtonHover += HandleButtonHover;
        canvas.worldCamera = myCamera;
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 3;
        canvas.planeDistance = 0.15f;
    }
  

    private void OnDisable()
    {
        ButtonHoverChecker.OnButtonHover -= HandleButtonHover;
    }

    void PlayVideo()
    {
        videoPlayer.Play();
        StartCoroutine(ActivateUi((float)videoPlayer.length));
    }

    IEnumerator ActivateUi(float time)
    {
        Debug.Log(time);
        yield return new WaitForSeconds(time/2);
        //foreach (Transform item in uiElementsHolder)
        //{
        //    item.gameObject.SetActive(true);
        //}
        uiElementsHolder.gameObject.SetActive(true);
        gameManager.cursor.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
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
        gameManager.cursor.SetActive(false);
      //  Cursor.visible = false;
        uiElementsHolder.gameObject.SetActive(false);
        //foreach (Transform item in uiElementsHolder)
        //{
        //    item.gameObject.SetActive(false);
        //}
    }
}
