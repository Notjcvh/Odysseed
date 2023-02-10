using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemAction {USE, EQUIP, DROP} // For Later might have to change
public class Inventory : MonoBehaviour
{

    public List<Item> items = new List<Item>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack; // For Later probably updating the game

    #region Singleton
    public static Inventory inventoryInstance;

    private void Awake()
    {
        if (inventoryInstance == null)
        {
            inventoryInstance = this;
            DontDestroyOnLoad(this); //For Later
        }
        else
            Destroy(this.gameObject);
    }
    #endregion
    
    public void AddItem(Item item)
    {
        items.Add(item);
        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke(); 
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }

}
