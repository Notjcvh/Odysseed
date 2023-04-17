using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public SceneData sceneToLoad;
    public Scene scene;
    public GameManager gameManager;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

            string sceneName = sceneToLoad.ToString();
      
            print(sceneName + "    " + scene.name);
            gameManager.LoadLevel(sceneName);
        }
    }


}
