
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/AmmoInfo")]
public class AmmoInfo : ItemInfo
{
    public AmmoType AmmoType = AmmoType.Rifle;
    
    
    protected override List<ItemComponent> GetComponents()
    {
        return new ItemComponent[]
        {
            new AmmoComponent(AmmoType)
        }.ToList();
    }
}
