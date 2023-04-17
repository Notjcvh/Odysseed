using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public void Playaudio(int i)
    {
        AudioSource.PlayClipAtPoint(audioClips[i], transform.position);
    }
}
