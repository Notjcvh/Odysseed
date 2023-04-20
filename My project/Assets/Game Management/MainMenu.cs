using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameManager gameManager;
    public SceneData scenetoLoad;
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        Cursor.visible = true;
    }

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StartGame()
    {
        gameManager.LoadLevel(scenetoLoad.ToString());
    }

    public void OpenSettings()
    {
        gameManager.gamePaused = true;
        //we want to puase the game to open up the pause menu
    }

    public void Quit()
    {
        gameManager.QuitGame();
    }


}
