using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        else
            DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public Equipment[] currentEquipment;

    public delegate void OnEquipmentChangedCallback();
    public OnEquipmentChangedCallback onEquipmentChangedCallback;

   

    private void Start()
    {
        int numSlots = System.Enum.GetNames(typeof(EquipType)).Length; // For Later this is cool
        currentEquipment = new Equipment[numSlots]; // this should work 
    }


    public void Equip(Equipment newEquipment)
    {
        int equipSlot = (int) newEquipment.equipType; // For Later

        Equipment oldEquipment = null;

        if (currentEquipment[equipSlot] != null) // if something is already inside 
        {
            oldEquipment = currentEquipment[equipSlot];
            Inventory.inventoryInstance.AddItem(oldEquipment); // For Later why can i call this so e
        }

        currentEquipment[equipSlot] = newEquipment;

        StatusManager.instance.UpdateCharacterStatus(newEquipment,oldEquipment);

        onEquipmentChangedCallback.Invoke();
    }
}
