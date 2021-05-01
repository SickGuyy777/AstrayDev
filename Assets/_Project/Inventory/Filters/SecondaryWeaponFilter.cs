
public class SecondaryWeaponFilter : InventoryFilter
{
    protected override ItemComponent[] GetFunctionalityWhitelist()
    {
        return new ItemComponent[]
        {
            new EquipSecondaryComponent()
        };
    }
}
