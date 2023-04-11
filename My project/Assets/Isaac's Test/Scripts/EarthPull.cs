using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPull : MonoBehaviour
{
    public float range = 30f;
    public bool hasPulled = false;
    public List<LineRenderer> newLineRenderer;
    public GameObject tendral;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!hasPulled)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            //find all nearby enemies with in a range
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
                    PullEnemies();
                }
            }
            hasPulled = true;
        }

        //pull all nearby enemies towards this gameobject
    }

    public void PullEnemies()
    {
        //Lerp enemy positions towards the center
    }
}