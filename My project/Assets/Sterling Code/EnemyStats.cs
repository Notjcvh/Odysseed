using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStats: MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Material myMaterial;
    


    public event System.Action<float> OnHealthPercentChange = delegate { };

    private void OnEnable()
    {
        currentHealth = maxHealth;
       
    }

    public void ModifiyHealth(int amount)
    {
        currentHealth += amount;
        float currentHealthPercent = (float) currentHealth / (float) maxHealth;
        OnHealthPercentChange(currentHealthPercent);

    }

    private void Update()
    {
        // the start
       if (Input.GetKeyDown(KeyCode.P))
        {
            ModifiyHealth(-20);
            StartCoroutine(VisualIndicator(Color.white));
        }
    }


    //Damage visual Indicator 
    private  IEnumerator VisualIndicator(Color color)
    {
        myMaterial.color = color;
        yield return new WaitForSeconds(0.15f);
        myMaterial.color = Color.red;

    }




    /*
    void TakeDamage(int number)
    {
        Debug.Log("Hit reciveved");

        //health -= number;
    }


    private void Update()
    {
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }*/


}
