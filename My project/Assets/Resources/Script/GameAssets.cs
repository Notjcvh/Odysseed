using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject gruntEnemies;
    public GameObject playerUI;
    public GameObject SceneTransitionCanvas;
    public GameObject BoneYard;
    public GameObject hit1;
}
