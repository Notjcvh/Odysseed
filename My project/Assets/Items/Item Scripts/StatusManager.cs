using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{

    public CharacterStatus playerStatus;

    #region Singleton

    public static StatusManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion

    /*
     * 
     *   playerStatus.armourHearts -= oldEquipment.armourModifier;
            playerStatus.speed -= oldEquipment.speedModifier;
            playerStatus.attackSpeed -= oldEquipment.attackSpeedModifier;
            playerStatus.strength -= oldEquipment.strengthModifier;
            playerStatus.knockbackValue -= oldEquipment.knockbackModifier;
            playerStatus.stun -= oldEquipment.stunModifier;
     * 
     * 
    */

    public void UpdateCharacterStatus(Equipment newEquipment, Equipment oldEquipment)
    {
        /*
        if (oldEquipment != null)
        {
            playerStatus.armourHearts -= oldEquipment.armourModifier;
            playerStatus.speed -= oldEquipment.speedModifier;
            playerStatus.attackSpeed -= oldEquipment.attackSpeedModifier;
            playerStatus.strength -= oldEquipment.strengthModifier;
            playerStatus.knockbackValue -= oldEquipment.knockbackModifier;
            playerStatus.stun -= oldEquipment.stunModifier;
        }
        */

        //just use this 
        playerStatus.speed = playerStatus.baseSpeed + newEquipment.speedModifier;
        playerStatus.attackSpeed = playerStatus.baseAttackSpeed + newEquipment.attackSpeedModifier;
        playerStatus.strength = playerStatus.baseStrength + newEquipment.strengthModifier;
        playerStatus.knockbackValue = playerStatus.baseKnockbackValue + newEquipment.knockbackModifier;
        playerStatus.stun = playerStatus.baseStun + newEquipment.stunModifier;

    }
}
