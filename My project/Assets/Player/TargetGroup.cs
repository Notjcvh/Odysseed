using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetGroup : MonoBehaviour
{

    public CinemachineTargetGroup cinemachineTargets;
    public CinemachineVirtualCamera virtualCamera;
    public PlayerInput playerInput;

    public Queue<Transform> uncleanTargetGroup = new Queue<Transform>();
    private Queue<Transform> cleanTargetGroup = new Queue<Transform>();
    private HashSet<Transform> filter = new HashSet<Transform>();

    private void Start()
    {
        cinemachineTargets = GetComponent<CinemachineTargetGroup>();
    }

    public void Add(Transform objLocation)
    {
        uncleanTargetGroup.Enqueue(objLocation);
        //Loops through (removing duplicates) and adds transforms to our 3 collection 
        while (uncleanTargetGroup.Count > 0)
        {
            Transform current = uncleanTargetGroup.Dequeue();
            if (filter.Contains(current))
            {
                continue;
            }
            filter.Add(current);
            cleanTargetGroup.Enqueue(current);
            cinemachineTargets.AddMember(current, 1, 0); // this one is in the CinemachineTarget group componenet
        }

        virtualCamera.enabled = true;
    }
    

    // Once if Player Manger != jump then 
    public void EmptyAllCollections()
    {
        if(Input.GetKey(KeyCode.Backspace))
        {
            while (cinemachineTargets.m_Targets.Length > 0 )
            {
                Transform current = cleanTargetGroup.Dequeue();
                if(current.gameObject.tag == "Player")
                {
                    cleanTargetGroup.Enqueue(current);
                    continue;
                }            
                filter.Clear();
                cinemachineTargets.RemoveMember(current); // this one is in the CinemachineTarget group componenet
            }
            virtualCamera.enabled = false;
        }
    }




}

