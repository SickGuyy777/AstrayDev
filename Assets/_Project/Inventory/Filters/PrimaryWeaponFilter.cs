
public class PrimaryWeaponFilter : InventoryFilter
{
    protected override ItemComponent[] GetFunctionalityWhitelist()
    {
        return new ItemComponent[]
        {
            new EquipPrimaryComponent()
        };
    }
}
