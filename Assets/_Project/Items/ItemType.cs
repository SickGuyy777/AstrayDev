using System.Collections.Generic;

public static class ItemTypeContainer
{
    public static Dictionary<SetItemType, ItemType> ItemTypeDictionary = new Dictionary<SetItemType, ItemType>();
    public static readonly ItemType None = new ItemType(new ItemFunctionality());
    public static readonly ItemType PrimaryWeapon = new ItemType(new ItemFunctionality(), new EquipPrimaryFunctionality());
    public static readonly ItemType SecondaryWeapon = new ItemType(new ItemFunctionality(), new EquipSecondaryFunctionality());


    static ItemTypeContainer()
    {
        ItemTypeDictionary.Add(SetItemType.None, None);
        ItemTypeDictionary.Add(SetItemType.PrimaryWeapon, PrimaryWeapon);
        ItemTypeDictionary.Add(SetItemType.SecondaryWeapon, SecondaryWeapon);
    }
    
    public enum SetItemType
    {
        None, PrimaryWeapon, SecondaryWeapon
    }
}

public class ItemType
{
    public List<Functionality> Functionalities = new List<Functionality>();


    public ItemType(params Functionality[] functionalities)
    {
        if(functionalities == null)
            return;
        
        foreach (Functionality functionality in functionalities)
        {
            Functionalities.Add(functionality);
        }
    }
    
    public ItemType Copy() => new ItemType(Functionalities.ToArray());
}


