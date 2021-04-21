
public class PrimaryWeaponFilter : InventoryFilter
{
    protected override Functionality[] GetFunctionalityWhitelist()
    {
        return new Functionality[]
        {
            new EquipPrimaryFunctionality()
        };
    }
}
