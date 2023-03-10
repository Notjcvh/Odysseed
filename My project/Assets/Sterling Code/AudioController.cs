using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class AudioController : MonoBehaviour
{
    //members

    public static AudioController instance;
    [SerializeField] Slider MasterVolumeSlider; // Maybe we can ge the slider value f
    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;

    public AudioSource MusicVolume;
    public AudioSource SFXVolume;
    public AudioSource Player1PuyoSpawner;
    public AudioSource Player2PuyoSpawner;
    public AudioSource Player1PuyoFX;
    public AudioSource Player2PuyoFX;



    public bool debug;
    public AudioTrack[] tracks;

    private Hashtable AudioTable; //relationship between audio types (key) and audio tracks (value)
    private Hashtable JobTable; //relationship between audio types (key) and jobs (value) (Coroutine,IEnumerator)

    [System.Serializable]
    public class AudioObject
    {
        public AudioType type;
        public AudioClip clip;
    }

    [System.Serializable]
    public class AudioTrack
    {
        public AudioSource source; 
        public AudioObject[] audio;
    }

    private class AudioJob
    {
        public AudioAction action;
        public AudioType type;
        public bool fade;
        public float delay;
        public bool looping;

        //Constuctor 
        public AudioJob(AudioAction _action, AudioType _type, bool _fade, float _delay, bool _looping)
        {
            //assigning values what those default values will be
            action = _action;
            type = _type;
            fade = _fade;
            delay = _delay;
            looping = _looping;
        }

    }

    private enum AudioAction
    {
        START,
        STOP,
        RESTART,
    }


    #region Unity Functions

    private void Awake()
    {
        if (!instance)
        {
            Configure();
            DontDestroyOnLoad(this);
        }
    }

    private void OnDisable()
    {
        Dispose();
    }

    #endregion

    #region Public Functions

    public void PlayAudio(AudioType type, bool fade = false, float delay = 0.0f, bool looping = false)
    {
        AddJob(new AudioJob(AudioAction.START, type, fade, delay, looping));
    }

    public void StopAudio(AudioType type, bool fade = false, float delay = 0.0f, bool looping = false)
    {
        AddJob(new AudioJob(AudioAction.STOP, type, fade, delay, looping));

    }

    public void RestartAudio(AudioType type, bool fade = false, float delay = 0.0f, bool looping = false)
    {
        AddJob(new AudioJob(AudioAction.RESTART, type, fade, delay, looping));

    }

    public AudioClip GetAudioClipFromAudioTrack(AudioType type, AudioTrack track)
    {
        foreach (AudioObject obj in track.audio)
        {
            if (obj.type == type)
                return obj.clip;
        }

        return null;
    }

    public void ChangeMasterVolume()
    {
        AudioListener.volume = MasterVolumeSlider.value;
        Debug.Log("Master Volume Slider set to: " + MasterVolumeSlider.value);
        Debug.Log("Master Volume Source set to: " + AudioListener.volume);

    }

    public void ChangeMusicVolume()
    {
        MusicVolume.volume = MusicVolumeSlider.value;
        //MusicVolumeSlider.value = MusicVolume.volume;
        Log("Music Volume Slider set to: " + MusicVolumeSlider.value);
        Log("Music Volume Source set to: " + MusicVolume.volume);
    }

    public void ChangeSFXVolume()
    {
        SFXVolume.volume = SFXVolumeSlider.value;
        Player1PuyoSpawner.volume = SFXVolumeSlider.value;
        Player2PuyoSpawner.volume = SFXVolumeSlider.value;
        Player1PuyoFX.volume = SFXVolumeSlider.value;
        Player2PuyoFX.volume = SFXVolumeSlider.value;

        Log("SFX Volume Slider set to: " + SFXVolumeSlider.value);
        Log("SFX Volume Source set to: " + SFXVolume.volume);
    }


    #endregion

    #region Private Functions

    private void Configure()
    {
        instance = this;
        AudioTable = new Hashtable();
        JobTable = new Hashtable();
        GenerateAudioTable();

        ChangeMasterVolume();
        ChangeMusicVolume();
        ChangeSFXVolume();

    }

    private void Dispose()
    {
        foreach (DictionaryEntry entry in JobTable)
        {
            IEnumerator job = (IEnumerator)entry.Value;
            StopCoroutine(job);
        }
    }

    private void GenerateAudioTable()
    {
        foreach (AudioTrack track in tracks)
        {
            foreach (AudioObject obj in track.audio)
            {
                //don't duplicate keys
                if (AudioTable.ContainsKey(obj.type))
                {
                    LogWarning("You're trying to register audio [" + obj.type + "] that has already been registered.");
                }
                else
                {
                    AudioTable.Add(obj.type, track);
                    Log("Registering audio [" + obj.type + "]. ");
                }
            }
        }
    }

    private void AddJob(AudioJob job)
    {
        //remove conflicting jobs
        RemoveConflictingJobs(job.type);

        //start job
        IEnumerator jobRunner = RunAudioJob(job);
        JobTable.Add(job.type, jobRunner);
        StartCoroutine(jobRunner);
        Log("Starting Job [" + job.type + "] with operation: " + job.action);

    }

    private void RemoveJob(AudioType type)
    {
        if (!JobTable.ContainsKey(type))//If a job of the passed in type is already running, throw warning and return.
        {
            LogWarning("Trying to stop a job [" + type + "] that does not exist.");
            return;
        }

        IEnumerator runningJob = (IEnumerator)JobTable[type];
        StopCoroutine(runningJob);
        JobTable.Remove(type);

    }

    private void RemoveConflictingJobs(AudioType type)
    {
        if (JobTable.ContainsKey(type)) //The issue first starts here, the JobTable is null...
        {
            RemoveJob(type);
        }

        AudioType conflictAudio = AudioType.None; //Will be set to something if there's a conflict

        foreach (DictionaryEntry entry in JobTable)
        {
            AudioType audioType = (AudioType)entry.Key;
            AudioTrack audioTrackInUse = (AudioTrack)AudioTable[audioType];
            AudioTrack audioTrackNeeded = (AudioTrack)AudioTable[type];

            if (audioTrackInUse.source == audioTrackNeeded.source)
            {
                //There's a conflict!
                conflictAudio = audioType;
            }
        }

        if (conflictAudio != AudioType.None) //If there's a conflict
        {
            RemoveJob(conflictAudio);
        }
    }



    private void Log(string msg)
    {
        if (!debug) return;
        Debug.Log("[Audio Controller]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!debug) return;
        Debug.LogWarning("[Audio Controller]: " + msg);
    }

    private IEnumerator RunAudioJob(AudioJob job)
    {
        yield return new WaitForSeconds(job.delay);

        AudioTrack track = (AudioTrack)AudioTable[job.type];
        track.source.clip = GetAudioClipFromAudioTrack(job.type, track);

        switch (job.action)
        {
            case AudioAction.START:
                track.source.loop = job.looping;
                track.source.Play();
                break;

            case AudioAction.STOP:
                if (!job.fade)
                {
                    track.source.Stop();
                }
                break;

            case AudioAction.RESTART:
                track.source.Stop();
                track.source.Play();
                break;
        }

        if (job.fade)
        {
            float initial = job.action == AudioAction.START || job.action == AudioAction.RESTART ? 0.0f : 1.0f;
            float target = initial == 0 ? 1.0f : 0.0f;
            float duration = 1.0f; //parameterize?
            float timer = 0.0f;

            while (timer <= duration)
            {
                track.source.volume = Mathf.Lerp(initial, target, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            if (job.action == AudioAction.STOP)
            {
                track.source.Stop();
            }
        }

        JobTable.Remove(job.type);
        Log("Current job count: " + JobTable.Count);

        yield return null;
    }

    #endregion
}