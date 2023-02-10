using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStatusData", menuName = "StatusObjects/Player", order = 1)] // For later might want to rename 
public class CharacterStatus : ScriptableObject
{
    // Base Stats
    public string characterName = "name";
    public int health = 0;

    
    public int baseSpeed = 0;
    public int baseAttackSpeed = 0;
    public int baseStrength = 0; // damage dealtFF
    public int baseKnockbackValue = 0; // how far does an attack push the enemy 
    public int baseStun = 0;



    public int armourHearts = 0; 
    public int speed = 0;
    public int attackSpeed = 0;
    public int strength = 0; // damage dealtFF
    public int knockbackValue = 0; // how far does an attack push the enemy 
    public int stun = 0;
    public float focusMeter = 0; // For later, this is not upgradable
}
