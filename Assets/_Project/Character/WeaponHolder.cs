using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private Transform handPos;
    [SerializeField] private Inventory[] weaponInventories;
    
    public Weapon HoldingWeapon { get; private set; }

    private List<WeaponComponent> weaponComponents = new List<WeaponComponent>();
    private bool hasWeapon => weaponComponents.Count > 0;

    private int weaponIndex;
    private int currentWeaponIndex
    {
        get => weaponIndex;

        set
        {
            if (value > weaponComponents.Count - 1)
                value = 0;
            else if (value < 0)
                value = weaponComponents.Count - 1;

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
    
    private void Update()
    {
        if(HoldingWeapon != null && !hasWeapon)
            UnEquipWeapon();
    }
    
    private void UpdateWeapons()
    {
        weaponComponents.Clear();

        foreach (Inventory inventory in weaponInventories)
        {
            if(inventory.Slots == null)
                continue;
            
            foreach (Slot slot in inventory.Slots)
            {
                Item item = slot.Item;
                if(item.IsEmpty || item == null)
                    continue;

                WeaponComponent weaponComponent = item.GetComponent<WeaponComponent>();
                
                if(weaponComponent != null)
                    weaponComponents.Add(weaponComponent);
            }
        }
        
        EquipWeapon(currentWeaponIndex);
    }
    
    public void EquipWeapon(int index)
    {
        if(!hasWeapon)
            return;
        
        if(HoldingWeapon != null)
            Destroy(HoldingWeapon.gameObject);
        
        currentWeaponIndex = index;
        WeaponComponent weaponComponent = weaponComponents[currentWeaponIndex];
        Weapon createdWeapon = weaponComponent.Instantiate(handPos.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z), handPos);
        HoldingWeapon = createdWeapon;
    }

    public void UnEquipWeapon()
    {
        if(HoldingWeapon == null || hasWeapon)
            return;
        
        if(HoldingWeapon != null)
            Destroy(HoldingWeapon.gameObject);
        
        HoldingWeapon = null;
    }

    public void ScrollEquip(int addIndex) => EquipWeapon(currentWeaponIndex + Mathf.Clamp(addIndex, -1, 1));
}
