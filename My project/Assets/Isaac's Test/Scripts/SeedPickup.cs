using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPickup : MonoBehaviour
{
    private AbilityHandler abilityHandler;
    public int abilityIndex;
    // Start is called before the first frame update
    void Start()
    {
        abilityHandler = GameObject.FindGameObjectWithTag("AbilityHandler").GetComponent<AbilityHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        abilityHandler.AddAbility(abilityIndex);
        Destroy(this.gameObject);
    }
}
