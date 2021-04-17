using System.Collections.Generic;
using System.Linq;

public static class ItemTypeContainer
{
    public static Dictionary<SetItemType, ItemType> ItemTypeDictionary = new Dictionary<SetItemType, ItemType>();
    public static readonly ItemType None = new ItemType();
    public static readonly ItemType Weapon = new ItemType(new EquipPrimaryFunctionality());


    static ItemTypeContainer()
    {
        ItemTypeDictionary.Add(SetItemType.None, None);
        ItemTypeDictionary.Add(SetItemType.Weapon, Weapon);
    }
    
    public enum SetItemType
    {
        None, Weapon
    }
}

public class ItemType
{
    public List<Functionality> Functionalities = new List<Functionality>();


    public ItemType(params Functionality[] functionalities)
    {
        Functionalities = functionalities.ToList();
    }
}


