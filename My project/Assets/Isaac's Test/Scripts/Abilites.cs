using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class Abilites : MonoBehaviour
{
    public class MyFloatEvent : UnityEvent<float> { }
    public MyFloatEvent OnAbilityUse = new MyFloatEvent();
    [Header("Ability Info")]
    public string title;
    public float cooldownTime = 1;
    public AbilityCooldown abilityCooldown;
    private bool canUse = true;
    public void TriggerAbility()
    {
        if (canUse)
        {
            OnAbilityUse.Invoke(cooldownTime);
            Ability();
            StartCooldown();
        }

    }
    public abstract void Ability();
    void StartCooldown()
    {
        StartCoroutine(Cooldown());
        abilityCooldown.cooldownTime = cooldownTime;
        abilityCooldown.UseSpell();
        IEnumerator Cooldown()
        {
            canUse = false;
            yield return new WaitForSeconds(cooldownTime);
            canUse = true;
        }
    }
}
