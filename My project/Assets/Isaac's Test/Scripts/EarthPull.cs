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
    // Start is called before the first frame update
    private void Start()
    {
        endPosition = this.transform.position;
    }
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
                    PullEnemies(enemy);
                }
            }
            hasPulled = true;
        }
    }

    public void PullEnemies(GameObject enemy)
    {
        Vector3 targetDirection = (this.transform.position - enemy.transform.position).normalized;
        NavMeshAgent nav = enemy.gameObject.GetComponent<NavMeshAgent>();
        StartCoroutine(DisablingAI(nav));
        nav.enabled = false;
        enemy.transform.position = Vector3.Lerp(enemy.transform.position, this.transform.position, 100f);

        targetDirection.y = 0;
        IEnumerator DisablingAI(NavMeshAgent nav)
        {
            yield return new WaitForSeconds(1f);
            nav.enabled = true;
        }
    }
}
