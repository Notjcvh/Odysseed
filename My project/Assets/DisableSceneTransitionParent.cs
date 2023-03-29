using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSceneTransitionParent : MonoBehaviour
{
    public GameObject parent;
    private void OnDisable()
    {
        parent.SetActive(false);
    }
}
