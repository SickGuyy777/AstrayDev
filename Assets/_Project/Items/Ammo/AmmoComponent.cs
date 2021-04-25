
public class AmmoComponent : ItemComponent
{
    public AmmoType AmmoType;

    public AmmoComponent(AmmoType ammoType) => this.AmmoType = ammoType;

    public void SetAmmoType(AmmoType ammoType) => this.AmmoType = ammoType;

    public bool IsSameType(AmmoType ammoType) => this.AmmoType == ammoType;
    
    public override ItemComponent Copy() => new AmmoComponent(this.AmmoType);
}

public enum AmmoType
{
    Rifle, Pistol, Shell
}
