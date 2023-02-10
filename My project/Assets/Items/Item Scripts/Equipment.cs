using UnityEngine;


public enum EquipType {Seed} // For Later is there way to match a string type to enum type

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class Equipment : Item
{

    public EquipType equipType;


    //For Later what is the math behind it we might need to use floats 

    public int armourModifier; // for armoured hearts
    public int speedModifier;
    public int attackSpeedModifier;
    public int strengthModifier; // damage dealtFF
    public int knockbackModifier; // how far does an attack push the enemy 
    public int stunModifier;


    public override void Use()
    {
        base.Use();
        //EquipmentManager.instance.Equip(This) 
    }
}
