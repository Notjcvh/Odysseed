using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    private AudioController audioController;

    void Start()
    {
        audioController = (AudioController)FindObjectOfType(typeof(AudioController));
    }

    public void MenuSelect()
    {
        int whichSound = Random.Range(0, 2);

        switch (whichSound)
        {
            case 0:
                audioController.PlayAudio(AudioType.MenuSelect1, false, 0, false);
                break;
            case 1:
                audioController.PlayAudio(AudioType.MenuSelect2, false, 0, false);
                break;
            case 2:
                audioController.PlayAudio(AudioType.MenuSelect3, false, 0, false);
                break;
        }
    }

    public void CharacterQuip_NAME_()
    {
        int whichSound = Random.Range(0, 2);

        switch (whichSound)
        {
            case 0:
                //audioController.PlayAudio(AudioType.CHAR_QUIP, false, 0, false);
                break;
        }
    }
}
