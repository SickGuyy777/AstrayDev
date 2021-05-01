using System;
using TMPro;
using UnityEngine;

public class GunDisplaySlot : ItemDisplaySlot
{
    [SerializeField] private TMP_Text ammoAmount;
    private Inventory ammoSupply;

    protected override void UpdateDisplay()
    {
        base.UpdateDisplay();
        base.UpdateDisplay();
        
        Inventory supply = inventory.CompoundInventory == null ? inventory : inventory.CompoundInventory.CompoundedInventory;
        if(ammoSupply == null && supply != null)
            supply.OnChanged += this.UpdateDisplay;
        
        ammoAmount.text = GetAmmoAmount(supply).ToString("0");
        ammoSupply = supply;
    }

    private int GetAmmoAmount(Inventory supply)
    {
        int amount = 0;

        if (supply == null)
            return 0;
        
        GunComponent gunComponent = itemReference.GetComponent<GunComponent>();

        if (gunComponent == null)
            return 0;
        
        foreach (AmmoComponent ammoComponent in supply.GetItemsOfType<AmmoComponent>())
        {
            if (ammoComponent.IsSameType(gunComponent.ammoType))
            {
                amount += ammoComponent.Item.Amount;
            }
        }

        return amount;
    }

    private void OnDestroy()
    {
        if(ammoSupply != null)
            ammoSupply.OnChanged -= this.UpdateDisplay;
    }
}
