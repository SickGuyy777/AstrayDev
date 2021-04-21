using UnityEngine;

[CreateAssetMenu(menuName = "Items/GunInfo")]
public class GunInfo : ItemInfo
{
    public GunType gunType = GunType.Primary;
    public GameObject gunPrefab;
    
    
    protected override Functionality[] GetFunctionalities()
    {
        Functionality[] functionalities = null;
            
        switch (gunType)
        {
            case GunType.Primary:
                functionalities = new Functionality[]
                {
                    new ItemFunctionality(),
                    new GunFunctionality(gunPrefab),
                    new EquipPrimaryFunctionality(), 
                };
                break;
            case GunType.Secondary:
                functionalities = new Functionality[]
                {
                    new ItemFunctionality(),
                    new GunFunctionality(gunPrefab),
                    new EquipSecondaryFunctionality(), 
                };
                break;
        }

        return functionalities;
    }
}

public enum GunType
{
    Primary, Secondary
}


