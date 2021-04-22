using UnityEngine;

public class WeaponComponent : ItemComponent
{
    public Weapon weaponPrefab;

    
    public WeaponComponent(Weapon weaponPrefab) => SetWeaponPrefab(weaponPrefab);

    public void SetWeaponPrefab(Weapon aGunPrefab) => this.weaponPrefab = aGunPrefab;

    public Weapon SpawnWeapon(Transform handPos)
    {
        Weapon gunObj = GameObject.Instantiate(weaponPrefab, handPos.position, Quaternion.identity);
        
        return gunObj;
    }
}
