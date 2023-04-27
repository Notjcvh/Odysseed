using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Tutorial : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TutorialPopUp promptHolder;
    public Button forwardButton;
    public Button backButton;
    public int index;
    public int fullIndex;
    public void SetPlaceInIndex()
    {
        promptHolder = GetComponentInParent<TutorialPopUp>();
        foreach (Transform item in promptHolder.childTransforms)
        {
            if (item.name == this.name)
            {
                index = promptHolder.childTransforms.IndexOf(item);
            }
            else
            {
                continue;
            }
        }

        //determine the position of the object within the parent array
        if(index == 0)
        {
            Debug.Log(this.name + " is in the start of an array");
            if (promptHolder.childTransforms.Count > 1)
            {
                backButton.interactable = false;
                backButton.gameObject.SetActive(false);
            }
            else
            {
                forwardButton.interactable = false;
                forwardButton.gameObject.SetActive(false);
                backButton.interactable = false;
                backButton.gameObject.SetActive(false);
            }
        }
        // to ccheck if the parent array length if it is odd or even, then we check what position we are in 
        else if(index >= 1 && index <= promptHolder.childTransforms.Count - 2)
        {
         //   Debug.Log(this.name + " is in the middle of an array");
            forwardButton.interactable = true;
            backButton.interactable = true;
        }
        else
        {
         //   Debug.Log(this.name + " is at the end of an array");
            forwardButton.interactable = false;
            forwardButton.gameObject.SetActive(false);
        }       
    }

    #region Events 
    public void Continue()
    {
        promptHolder.NextPrompt();
    }
    public void Back()
    {
        promptHolder.LastPrompt();
    }
    #endregion



    // Implement the IPointerEnterHandler interface
    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    // Implement the IPointerExitHandler interface
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
