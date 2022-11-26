using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject sceneTransition;
    public TextMeshProUGUI displayText;
    public VectorValue room;
    public SceneManager currentScene;
    public float textDisappearTimer = 1.3f;

    public GameObject player;
    private PlayerMovement playerMovement;





    private void Awake()
    {
       sceneTransition = Instantiate(GameAssets.i.SceneTransitionCanvas);
       DisplayText(sceneTransition);
       playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.stopMovementEvent = true;

    }

    private void Update()
    {
        Image panel = displayText.GetComponentInParent<Image>();
        textDisappearTimer -= Time.deltaTime;

        if (textDisappearTimer < 0)
        {
             playerMovement.stopMovementEvent = false;
        }
    }

    #region Public Functions
    public void DisplayText(GameObject scene)
    {
      
        if (sceneTransition != null)
        {
            foreach (Transform t in scene.transform)
            {
                t.gameObject.SetActive(true);

            }
            displayText = scene.GetComponentInChildren<TextMeshProUGUI>();
            displayText.SetText(room.description);
        }
        else
            return;       
    }
    #endregion
}
