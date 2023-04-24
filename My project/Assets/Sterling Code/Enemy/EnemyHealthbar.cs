using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthbar : MonoBehaviour
{

    public Image foregroundImage;
    public Image middlegroundImage;
    public Enemy enemy;
    public bool isStatic;

    public float updateSpeedInSeconds1 = 0.2f;
    public float updateSpeedInSeconds2 = 0.5f;

    private void Awake()
    {
        //GetComponentInParent<Enemy>().OnHealthPercentChange += HandleHealthChange;
    }
    

    public void HandleHealthChange(float percent)
    {
        // using coroutine for smoothing 
        StartCoroutine(ChangeToPercent(percent));
    }

    private IEnumerator ChangeToPercent(float percent)
    {
        float percentChange = foregroundImage.fillAmount;
        float percentChange2 = middlegroundImage.fillAmount;
        float elapsed = 0f;
        while(elapsed < updateSpeedInSeconds2)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(percentChange, percent, elapsed / updateSpeedInSeconds1);
            middlegroundImage.fillAmount = Mathf.Lerp(percentChange2, percent, elapsed / updateSpeedInSeconds2);
            yield return null;
        }
       foregroundImage.fillAmount = percent;
       middlegroundImage.fillAmount = percent;
    }

    private void LateUpdate()
    {
        if(!isStatic)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}
