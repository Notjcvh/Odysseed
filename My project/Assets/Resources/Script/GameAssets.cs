using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameAssets : MonoBehaviour
{
    //i is short for instancing 
    private static GameAssets _i;


    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;

        }
    }

    public Transform DamageTextHolder;
    public GameObject hit1;
    public GameObject smokePoof;

}
