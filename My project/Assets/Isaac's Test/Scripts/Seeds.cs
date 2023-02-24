using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeds : MonoBehaviour
{
    public int armourHearts = 0;
    public int speed = 0;
    public int attackSpeed = 0;
    public int strength = 0; // damage dealtFF
    public int knockbackValue = 0; // how far does an attack push the enemy 
    public int stun = 0;
    public float focusMeter = 0; // For later, this is not upgradable

    public int[] GetAttributes()
    {
        int[] seedAtributes = {armourHearts, speed, attackSpeed, strength, knockbackValue, stun};
        return seedAtributes;
    }
    public void SpecialAbility()
    {
        Debug.Log("Cast Ability");
    }
    public void UltimateAbility()
    {
        Debug.Log("Cast Ability");
    }


}
