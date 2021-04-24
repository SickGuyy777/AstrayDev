
using UnityEngine;

[CreateAssetMenu(menuName = "Items/AmmoInfo")]
public class AmmoInfo : ItemInfo
{
    public AmmoType AmmoType = AmmoType.Rifle;
    
    
    protected override ItemComponent[] GetComponents()
    {
        return new ItemComponent[]
        {
            new AmmoComponent(AmmoType)
        };
    }
}
