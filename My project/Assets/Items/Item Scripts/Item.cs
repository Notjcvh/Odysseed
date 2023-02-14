using UnityEngine;

public enum ItemType {heal, equip} // For Later 


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{

    new public string name = "New Item";
    public Sprite icon;
    public ItemType type;
    public virtual void Use()
    {
        // This function is suppose to be overriden 
    }

    public virtual void Drop()
    {
        // Will be used for item inventory not seeds, get instance of item then rmeove from inventory 
        // this function is suppose to be overriden 
        // further functionality is needed 
    }
}
