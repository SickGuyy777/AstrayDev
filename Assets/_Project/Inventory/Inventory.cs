using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Item[] items = new Item[30];

    public Item[] Items => items;
    private InventoryFilter filter;
    public Action OnChanged;


    private void Awake()
    {
        filter = GetComponent<InventoryFilter>();
    }

    public bool CanAdd(Item itemToAdd) => filter == null ||  filter != null && filter.ContainedInWhitelist(itemToAdd);

    public bool AddToInventory(Item itemToAdd, int slotIndex = -1, int amount = -1)
    {
        if (amount == -1)
            amount = itemToAdd.Amount;

        int endAmount = itemToAdd.Amount - amount;

        if (!CanAdd(itemToAdd))
        {
            Debug.Log(":(");
            return false;
        }
            
        
        while(itemToAdd.Amount > endAmount)
        {
            int bestSlotIndex = GetBestSlot(itemToAdd.Info);

            if (slotIndex != -1)
            {
                Item item = Items[slotIndex];
                if (IsSameItem(itemToAdd.Info, item.Info) && !item.IsFull)
                    bestSlotIndex = slotIndex;
                if (item.IsEmpty)
                    bestSlotIndex = slotIndex;
            }

            if (bestSlotIndex == -1)
                break;
            
            Items[bestSlotIndex].Add(itemToAdd, 1);
        }

        OnChanged?.Invoke();
        return true;
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
        for (int i = 0; i < Items.Length; i++)
        {
            Item item = Items[i];

            bool sameItemSlot = IsSameItem(item?.Info, info) && !IsFull(i);

            if (sameItemSlot)
                return i;
        }
        
        for (int i = 0; i < Items.Length; i++)
        {
            if (IsEmpty(i))
                return i;
        }

        return -1;
    }
    
    private bool IsSameItem(ItemInfo info1, ItemInfo info2) => info1 == info2;

    private bool IsFull(int slotIndex) => Items[slotIndex].Amount >= Items[slotIndex].MaxStack;

    private bool IsEmpty(int slotIndex) => Items[slotIndex]?.Amount <= 0;
}
