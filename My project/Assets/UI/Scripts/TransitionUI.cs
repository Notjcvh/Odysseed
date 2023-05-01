using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionUI : SetCanvasRenderer
{
    public SceneHandler sceneHandler;

    public override void OnEnable()
    {
        base.OnEnable();
        sceneHandler = GameObject.FindGameObjectWithTag("Scene Handler").GetComponent<SceneHandler>();
    }

    /*
    public void InitializeScene()
    {
        sceneHandler.IntializeScene();
    }*/

}
