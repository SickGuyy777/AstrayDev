using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/WeaponInfo")]
public class WeaponInfo : ItemInfo
{
    public WeaponType weaponType = WeaponType.Primary;
    public Weapon weaponPrefab;
    
    
    protected override List<ItemComponent> GetComponents()
    {
        ItemComponent[] components = null;
            
        switch (weaponType)
        {
            case WeaponType.Primary:
                components = new ItemComponent[]
                {
                    new WeaponComponent(weaponPrefab),
                    new EquipPrimaryComponent(), 
                };
                break;
            case WeaponType.Secondary:
                components = new ItemComponent[]
                {
                    new WeaponComponent(weaponPrefab),
                    new EquipSecondaryComponent(), 
                };
                break;
        }

        return components.ToList();
    }
}

public enum WeaponType
{
    Primary, Secondary
}


