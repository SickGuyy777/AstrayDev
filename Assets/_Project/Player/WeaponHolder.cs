using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private Transform handPos;
    [SerializeField] private Inventory[] weaponInventories;
    
    public Weapon HoldingWeapon { get; private set; }
    
    private List<Weapon> weaponPrefabs = new List<Weapon>();
    private bool hasWeapon => weaponPrefabs.Count > 0;

    private int weaponIndex;
    private int currentWeaponIndex
    {
        get => weaponIndex;

        set
        {
            if (value > weaponPrefabs.Count - 1)
                value = 0;
            else if (value < 0)
                value = weaponPrefabs.Count - 1;

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
        weaponPrefabs.Clear();

        foreach (Inventory inventory in weaponInventories)
        {
            if(inventory.Items == null)
                continue;
            
            foreach (Item item in inventory.Items)
            {
                if(item.IsEmpty || item == null)
                    continue;

                WeaponComponent weaponFunc = item.GetComponent<WeaponComponent>();
                
                if(weaponFunc != null)
                    weaponPrefabs.Add(weaponFunc.weaponPrefab);
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
        Weapon weaponPrefab = weaponPrefabs[currentWeaponIndex];
        Weapon createdWeapon = Instantiate(weaponPrefab, handPos.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90), transform);
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
