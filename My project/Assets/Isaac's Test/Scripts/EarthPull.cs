using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EarthPull : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float range = 30f;
    public bool hasPulled = false;
    public float desiredDuration = 50f;
    public List<LineRenderer> newLineRenderer;
    public GameObject tendral;

    private void Start()
    {
        endPosition = this.transform.position;
    }

    void Update()
    {
        
        if (!hasPulled)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                
                if (distanceToEnemy < range)
                {
                    //stun all the enemies
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    enemyScript.DisableAI();
                    GameObject tendralGO = Instantiate(tendral, this.transform);
                    EarthTendral tendralScript = tendralGO.GetComponent<EarthTendral>();
                    tendralScript.SetEnemy(enemy.transform);
                    //Pull enemies after a while
                    PullEnemies(enemy);
                }
            }
            hasPulled = true;
        }
    }

    public void PullEnemies(GameObject enemy)
    {
        enemy.gameObject.GetComponent<Enemy>().isStunned = true;
        enemy.transform.position = Vector3.Lerp(enemy.transform.position, this.transform.position, 100f);
    }
}
