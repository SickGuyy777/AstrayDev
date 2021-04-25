using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int inventorySize = 30;
    [SerializeField] private Slot[] slots;

    public Slot[] Slots => slots;
    private InventoryFilter filter;
    public System.Action OnChanged;

    
    private void Awake()
    {
        filter = GetComponent<InventoryFilter>();

        slots = new Slot[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            Slots[i] = new Slot();
        }
            
    }

    public bool IsItemTypeAllowed(Item itemToAdd) => filter == null ||  filter != null && filter.ContainedInWhitelist(itemToAdd);

    public bool IsFullForItemType(ItemInfo info) => GetBestSlot(info) == -1;

    public int AddToInventory(Item itemToAdd, Slot startingSlot = null, int amountToAdd = 0)
    {
        if (!IsItemTypeAllowed(itemToAdd) | IsFullForItemType(itemToAdd.Info))
            return itemToAdd.Amount;

        int bestSlotIndex = GetBestSlot(itemToAdd.Info);
        if (bestSlotIndex < 0)
            return itemToAdd.Amount;

        int remaining = 0;
        if (amountToAdd > itemToAdd.Amount)
            remaining = amountToAdd - itemToAdd.Amount;

        if (amountToAdd <= 0 || amountToAdd > itemToAdd.Amount)
            amountToAdd = itemToAdd.Amount;

        Slot bestSlot = Slots[bestSlotIndex];

        int i = 0;
        while (amountToAdd > 0)
        {
            i++;
            if (!bestSlot.IsEmpty && bestSlot.IsFull)
            {
                bestSlotIndex = GetBestSlot(itemToAdd.Info);
                
                if (bestSlotIndex < 0)
                {
                    remaining += amountToAdd;
                    break;
                }

                bestSlot = Slots[bestSlotIndex];
            }
            
            Slot chosenSlot = startingSlot != null && startingSlot.CanAdd(itemToAdd.Info) ? startingSlot : bestSlot;
            amountToAdd = chosenSlot.AddItem(itemToAdd, amountToAdd);

            if (i >= 100)
            {
                Debug.LogError("Would cause stack overflow");
                break;
            }
        }

        OnChanged?.Invoke();
        return remaining;
    }

    public List<T> GetItemsOfType<T>() where T : ItemComponent
    {
        List<T> listOfComponents = new List<T>();
        
        foreach (Slot slot in Slots)
        {
            if(slot.IsEmpty)
                continue;
            
            T component = slot.Item.GetComponent<T>();
            
            if(component != null)
                listOfComponents.Add(component);
        }

        return listOfComponents;
    }

    public void AddToInventory(Item[] itemsToAdd)
    {
        foreach (Item item in itemsToAdd)
        {
            AddToInventory(item);
        }
    }

    private int GetBestSlot(ItemInfo info)
    {
        int emptySlot = -1;
        for (int i = 0; i < Slots.Length; i++)
        {
            if (emptySlot < 0 && slots[i].IsEmpty)
            {
                emptySlot = i;
                continue;
            }

            if (slots[i].IsEmpty)
                continue;

            bool canAddToSlot = !Slots[i].IsFull && Slots[i].Item.IsSameItemType(info);

            if (canAddToSlot)
                return i;
        }

        if (emptySlot >= 0)
            return emptySlot;

        return -1;
    }
}
