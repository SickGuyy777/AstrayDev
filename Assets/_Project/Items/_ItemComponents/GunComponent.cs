
public class GunComponent : WeaponComponent
{
    public AmmoType ammoType;

    public GunComponent(Weapon weaponPrefab, AmmoType ammoType) : base(weaponPrefab)
    {
        this.ammoType = ammoType;
        this.weaponPrefab = weaponPrefab;
    }

    public override ItemComponent Copy() => new GunComponent(weaponPrefab, ammoType);
}
