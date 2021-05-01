using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    [SerializeField] private int size;
    [SerializeField] private Slot[] slots;
    private InventoryFilter filter;

    public CompoundInventory CompoundInventory { get; private set; }
    public Slot[] Slots => slots;
    
    public Action OnChanged;


    private void Awake()
    {
        filter = GetComponent<InventoryFilter>();
        
        Set(size);
    }

    public void Set(int size)
    {
        slots = new Slot[size];
        
        for (int i = 0; i < size; i++)
        {
            Slots[i] = new Slot();
            Slots[i].OnSlotChanged += Change;
        }

        OnChanged?.Invoke();
    }
    
    private void Change() => this.OnChanged?.Invoke();

    public void SetCompound(CompoundInventory compoundInventory) => this.CompoundInventory = compoundInventory;
    
    public bool IsItemTypeAllowed(Item itemToAdd) => filter == null ||  filter != null && filter.ContainedInWhitelist(itemToAdd);

    public bool IsFullForItemType(ItemInfo info) => GetBestSlot(info) == -1;

    public void AddToInventory(Item itemToAdd, Slot startingSlot = null, int amountToAdd = 0)
    {
        if (!IsItemTypeAllowed(itemToAdd) || IsFullForItemType(itemToAdd.Info))
            return;

        int bestSlotIndex = GetBestSlot(itemToAdd.Info);
        
        if (bestSlotIndex < 0)
            return;
        
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
                
                    break;

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
    }
    
    public void AddToSlot(Item itemToAdd, Slot slot, int amountToAdd = 0)
    {
        if (!IsItemTypeAllowed(itemToAdd) || !slot.CanAdd(itemToAdd.Info))
            return;
        
        if (amountToAdd <= 0 || amountToAdd > itemToAdd.Amount)
            amountToAdd = itemToAdd.Amount;

        slot.AddItem(itemToAdd, amountToAdd);
    }

    public Item[] GetItems() => Slots.Select(slot => slot.Item).ToArray();

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
