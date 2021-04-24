
public class AmmoComponent : ItemComponent
{
    public AmmoType AmmoType;

    public AmmoComponent(AmmoType ammoType)
    {
        this.AmmoType = ammoType;
    }

    public bool IsSameType(AmmoType ammoType) => this.AmmoType == ammoType;
}

public enum AmmoType
{
    Rifle, Pistol, Shell
}
