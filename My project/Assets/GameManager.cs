using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject sceneTransition;

    public TextMeshProUGUI displayText;
    public VectorValue room;

    public SceneManager currentScene;
    


    private void Awake()
    {
       sceneTransition = Instantiate(GameAssets.i.SceneTransitionCanvas);

        print(sceneTransition);
        DisplayText(sceneTransition);
       

    }

    public void ChangeScene(VectorValue scene)
    {
        foreach (Transform t in sceneTransition.transform)
        {
            t.gameObject.SetActive(true);
        }


        if (sceneTransition != null)
        {
            displayText = sceneTransition.GetComponentInChildren<TextMeshProUGUI>();
            displayText.text = scene.description;
        }
        else
            return;
    }

    public void DisplayText(GameObject scene)
    {
        foreach (Transform t in scene.transform)
        {
            t.gameObject.SetActive(true);
        }


        if (sceneTransition != null)
        {
            displayText = scene.GetComponentInChildren<TextMeshProUGUI>();
            displayText.text = room.description;
        }
        else
            return;       
    }

}