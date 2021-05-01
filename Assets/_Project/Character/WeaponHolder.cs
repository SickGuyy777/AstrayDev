using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private Transform handPos;
    [SerializeField] private Inventory[] weaponInventories;

    public Weapon HoldingWeapon => currentWeaponIndex >= 0 && currentWeaponIndex < equipedWeapons.Count ? equipedWeapons[currentWeaponIndex] : null;

    private List<Weapon> equipedWeapons = new List<Weapon>();
    private bool hasWeapon => equipedWeapons.Count > 0;

    private int weaponIndex = 0;
    private int currentWeaponIndex
    {
        get => weaponIndex;

        set
        {
            if (value >= equipedWeapons.Count)
                value = 0;
            else if(value < 0)
                value = equipedWeapons.Count - 1;

            weaponIndex = value;
        }
    }

    private void Awake() => SetupInventories();

    private void SetupInventories()
    {
        foreach (Inventory weaponInventory in weaponInventories)
        {
            weaponInventory.OnChanged += UpdateWeapons;
        }
    }

    private void UpdateWeapons()
    {
        DestroyEquipedWeapons();

        foreach (Inventory inventory in weaponInventories)
        {
            if(inventory.Slots == null)
                continue;
            
            foreach (Slot slot in inventory.Slots)
            {
                if(slot.IsEmpty)
                    continue;

                WeaponComponent weaponComponent = slot.Item.GetComponent<WeaponComponent>();

                if (weaponComponent != null)
                {
                    Weapon createdWeapon = weaponComponent.Instantiate(handPos.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z), handPos);
                    createdWeapon.gameObject.SetActive(false);
                    equipedWeapons.Add(createdWeapon);
                }
            }
        }
        
        EquipWeapon(currentWeaponIndex);
    }

    public void Primary(IWeaponArgsHolder holder) => HoldingWeapon?.Primary(holder);
    
    public void EquipWeapon(int index)
    {
        if(!hasWeapon)
            return;
        
        HoldingWeapon?.gameObject.SetActive(false);

        currentWeaponIndex = index;
        
        Weapon newWeapon = equipedWeapons[index];
        newWeapon.gameObject.SetActive(true);
        
        newWeapon.WaitCD(.2f);
    }

    public void UnEquipWeapon()
    {
        currentWeaponIndex = 0;
        HoldingWeapon?.gameObject.SetActive(false);
    }

    private void DestroyEquipedWeapons()
    {
        UnEquipWeapon();
        
        foreach (Weapon weapon in equipedWeapons)
        {
            Destroy(weapon.gameObject);
        }
        
        equipedWeapons.Clear();
    }

    public void ScrollEquip(int addIndex)
    {
        if(equipedWeapons.Count <= 1)
            return;
        
        HoldingWeapon?.gameObject.SetActive(false);

        currentWeaponIndex += addIndex;

        EquipWeapon(currentWeaponIndex);
    }
}
