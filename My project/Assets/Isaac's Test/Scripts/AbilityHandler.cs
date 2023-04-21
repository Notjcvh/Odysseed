using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    public List<Abilites> abilityList;
    public List<Abilites> abilitesToAdd;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }
    void Update()
    {
        if(playerInput.activateWaterSpecial && abilityList.Count >= 1)
        {
            abilityList[0].TriggerAbility();
        }
        if (playerInput.activateEarthSpecial && abilityList.Count >= 2)
        {
            abilityList[1].TriggerAbility();
        }
        if (playerInput.activateSunSpecial && abilityList.Count >= 3)
        {
            abilityList[2].TriggerAbility();
        }
    }
    public void AddAbility(int index)
    {
        abilityList.Add(abilitesToAdd[index]);
        if (abilityList.Count >= 1)
        {
            abilityList[0].abilityCooldown.isActive = true;
        }
        if (abilityList.Count >= 2)
        {
            abilityList[1].abilityCooldown.isActive = true;
        }
        if (abilityList.Count >= 3)
        {
            abilityList[2].abilityCooldown.isActive = true;
        }
    }
}
