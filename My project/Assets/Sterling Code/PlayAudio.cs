using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    private AudioController audioController;


    //Use Scriptable Object events to play the type of music

    void Start()
    {
        audioController = this.GetComponent<AudioController>();
    }

    public void MainMenu()
    {
        audioController.PlayAudio(AudioType.MainMenu, false, 0, true);
    }

  
}
