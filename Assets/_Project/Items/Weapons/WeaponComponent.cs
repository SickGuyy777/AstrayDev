using UnityEngine;

public class WeaponComponent : ItemComponent
{
    public Weapon weaponPrefab;

    
    public WeaponComponent(Weapon weaponPrefab) => SetWeaponPrefab(weaponPrefab);

    public void SetWeaponPrefab(Weapon aGunPrefab) => this.weaponPrefab = aGunPrefab;

    public Weapon Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        Weapon gunObj = GameObject.Instantiate(weaponPrefab, position, rotation, parent);
        gunObj.SetUp(Item);
        
        return gunObj;
    }
}
