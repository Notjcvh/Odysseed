using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Scenes/StartingPosition")]
public class VectorValue : ScriptableObject
{

    public string sceneName;
    public string levelName;
    public Vector3 initialStartValue;
    public Vector3 boneYard;
 


    [TextArea]
    public string description;


    public List<GameObject> doorsSequence;

    public void Add(GameObject door)
    {
        if (!doorsSequence.Contains(door))
            doorsSequence.Add(door);
        if (doorsSequence.Count > 0)
        {
            doorsSequence.Sort(delegate(GameObject a, GameObject b)
                {
                    return (a.GetComponent<DoorAnimation>().doorValue).CompareTo(b.GetComponent<DoorAnimation>().doorValue);
                });
            
        }
    }

    public void Remove(GameObject door)
    {
        if (doorsSequence.Contains(door))
            doorsSequence.Remove(door);
    }
}
