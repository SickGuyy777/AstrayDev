using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/GunInfo")]
public class GunInfo : WeaponInfo
{
    public AmmoType ammoType;
    
    
    protected override List<ItemComponent> GetComponents()
    {
        ItemComponent[] components = null;

        switch (weaponType)
        {
            case WeaponType.Primary:
                components = new ItemComponent[]
                {
                    new GunComponent(weaponPrefab, ammoType),
                    new EquipPrimaryComponent(),
                };
                break;
            case WeaponType.Secondary:
                components = new ItemComponent[]
                {
                    new GunComponent(weaponPrefab, ammoType),
                    new EquipSecondaryComponent(),
                };
                break;
        }

        return components.ToList();
    }
}
