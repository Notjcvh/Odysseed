using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorScript : MonoBehaviour
{
    public bool isStatic;
    public bool changeScale = false;


    public float minScale = 0.5f, maxScale = 1f;
    public float timeElapsed;
    public float scaleDuration;
    private Vector3 _currentScale;
    private Vector3 _stationaryScale;  // when the player is close to the indicatior



    // Start is called before the first frame update
    void Start()
    {
        _stationaryScale = new Vector3(maxScale, maxScale, maxScale);
    }

    // Update is called once per frame
    void Update()
    {
        _currentScale = transform.localScale;
        if (!isStatic)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
        
        if(changeScale)
        {
            // Reset timeElapsed when changing to a scaling mode
            timeElapsed = 0f;

            float scaleValue = Mathf.PingPong(Time.time, 1.0f); // set a value for scaling between 0 and 1
            float scale = Mathf.Lerp(minScale, maxScale, scaleValue); // use Lerp to interpolate the scale value between minScale and maxScale
            transform.localScale = new Vector3(scale, scale, scale); // apply the scale to the object
        }
        else
        {
            // Lerp to the current scale to max scale 
            float t = timeElapsed / scaleDuration;
            transform.localScale = Vector3.Lerp(_currentScale, _stationaryScale, t);
            timeElapsed += Time.deltaTime;
        }

        if(!this.gameObject.activeInHierarchy || transform.localScale == _stationaryScale)
        {
            timeElapsed = 0;
        }

    }




    
}
