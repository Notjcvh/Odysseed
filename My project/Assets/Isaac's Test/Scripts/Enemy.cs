using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;
using UnityEngine.Rendering;
public class Enemy : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject hitEffect;

    [Header("Animation")]
    public Animator animator;

    [Header("EnemyStatus")]
    public bool isStunned = false;
    public bool isTargeted = true;
    public event System.Action<float> OnHealthPercentChange = delegate { };

    [Header("Rooms")]
    public CombatRoom myRoom;

    [Header("Audio Caller")]
    public AudioController audioController;
    public AudioType playingAudio; // the currently playing audio
    [SerializeField] private AudioType queueAudio; // the next audio to play
    public AudioClip clip;
    public bool audioJobSent = false; // if job sent is true then it won't play
    private Dictionary<AudioType, AudioClip> ourAudio = new Dictionary<AudioType, AudioClip>();
    private List<AudioController.AudioObject> audioObjects = new List<AudioController.AudioObject>();

    [Header("Hit Effect")]
    public float blinkIntensity;
    public float blinkDuration;
    public SkinnedMeshRenderer[] mats;
    private float blinkTimer;


    private void Awake()
    {
        currentHealth = maxHealth;
        audioController = GetComponent<AudioController>();
    }


    private void Update()
    {
        if (currentHealth <= 0)
        {
            Vector3 enemyPos = this.transform.position;
            GameObject smokeEffect = Instantiate(GameAssets.i.smokePoof, new Vector3(enemyPos.x,enemyPos.y + 1 , enemyPos.z) , Quaternion.identity);
            if (myRoom != null)
            {
                myRoom.enemies.Remove(this.gameObject);
            }
            if (this.tag == "Boss")
            {
                animator.SetBool("IsDying", true);
            }
           /* else
            {
                this.gameObject.SetActive(false);
                DestroyImmediate(this.gameObject);
            }*/
            this.gameObject.SetActive(false);

            Destroy(smokeEffect, 1.5f);
            SelectAudio("Death");
            Destroy(this.gameObject);
              

        }


        /*
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = lerp * blinkIntensity;
        if(mats?.Length > 0)
        {
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].material.color = Color.white * intensity;
            }

        }*/
       
    }

    // the goal here is to 

    public void WhichRoom(GameObject room) 
    {
        GameObject location = room.gameObject;
        myRoom = location.GetComponent<CombatRoom>();  // finding the Combatroom script so we can run the functions when the enemy dies
    }

    public void ModifiyHealth(int amount)
    {
        GameObject hiteffs = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(hiteffs, 2f);
        currentHealth =currentHealth - amount;
        float currentHealthPercent = (float)currentHealth / (float)maxHealth;
        OnHealthPercentChange(currentHealthPercent);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("I was hit was in my name " + this.gameObject.name);
        DamagePopUp.Create(this.transform.position, damage);
        ModifiyHealth(damage);
        DisableAI();
        blinkTimer = blinkDuration;

       // GetComponent<EnemyStats>().VisualizeDamage(this.gameObject);
    }
    public void PlayTakeDamgage()
    {
        animator.SetBool("TakingDamage", true);
    }
    public void DisableAI()
    {
        this.isStunned = true;
    }


    #region Sound looping
    public void SelectAudio(string type)
    {
        if(type == "Bark")
        {
            float delay =  0;
            delay = Random.Range(0.5F, 10);
            ManageAudio(AudioType.RotEnemyNoise);
            StartCoroutine(WaitToPlay(delay));
        }
        else if(type == "Death")
        {
            ManageAudio(AudioType.RotDeath);
        }
    }
    public void ManageAudio(AudioType type)
    {
        if (ourAudio.Count < 1)
        {
            // Loop through each audio track
            foreach (AudioController.AudioTrack track in audioController.tracks)
            {
                // Access the audio objects in each track
                audioObjects.AddRange(track.audio);
                // Loop through each audio object in the track
                foreach (AudioController.AudioObject audioObject in audioObjects)
                {
                    // this should add all our audio to the dictionary
                    ourAudio.Add(audioObject.type, audioObject.clip);
                }
            }
        }
        if (ourAudio.ContainsKey(type))
        {
            clip = ourAudio[type];
        }
        if (type != playingAudio)
        {
            audioController.PlayAudio(type, false, 0, false);
            playingAudio = type;
        }
        else
        {
            audioController.PlayAudio(playingAudio, false, 0, false);
        }
    }

    IEnumerator WaitToPlay(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        audioJobSent = false;
    }

    #endregion
}
