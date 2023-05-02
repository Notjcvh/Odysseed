using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Checkpoint_UI : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;



    private void Start()
    {
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnLoopPointReached;
    }


    private void OnEnable()
    {
        videoPlayer.Play();
    }

    private void OnLoopPointReached(VideoPlayer player)
    {
        this.gameObject.SetActive(false);
    }
}
