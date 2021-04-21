
public class SecondaryWeaponFilter : InventoryFilter
{
    protected override Functionality[] GetFunctionalityWhitelist()
    {
        return new Functionality[]
        {
            new EquipSecondaryFunctionality()
        };
    }
}
