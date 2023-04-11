using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public string type;
    public int maxHealth;
    public int currentHealth;
    public Material myMaterial;


    //public event System.Action<float> OnHealthPercentChange = delegate { };

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }
    /*
    public void ModifiyHealth(int amount)
    {
        currentHealth += amount;
        float currentHealthPercent = (float)currentHealth / (float)maxHealth;
        OnHealthPercentChange(currentHealthPercent);
    }
    */

    public void VisualizeDamage(GameObject obj)
    {
        myMaterial = obj.gameObject.GetComponent<Renderer>().material;

        StartCoroutine(VisualIndicator(Color.white));
    }
    
    //Damage visual Indicator 
    protected virtual IEnumerator VisualIndicator(Color color)
    {
        myMaterial.color = color;
        yield return new WaitForSeconds(0.15f);
        myMaterial.color = Color.red;

    }



}
