using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : MonoBehaviour
{
    [SerializeField] private Item[] items = new Item[30];
    [SerializeField] private ItemTypeContainer.SetItemType[] functionalityWhiteList;
    
    public Item[] Items => items;
    private Functionality[] filter;


    private void Awake()
    {
        filter = EnumToFunctionalitys();
    }

    public bool CanAdd(Item itemToAdd) => ContainedInFilter(itemToAdd) && !itemToAdd.IsEmpty;

    public bool AddToInventory(Item itemToAdd, int slotIndex = -1)
    {
        if(!CanAdd(itemToAdd))
            return false;
        
        while(itemToAdd.Amount > 0)
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

    private bool ContainedInFilter(Item item)
    {
        foreach (Functionality functionality in filter)
        {
            if(item.ItemType.Functionalities.Contains(functionality))
                return true;
        }

        return false;
    }
    
    private Functionality[] EnumToFunctionalitys()
    {
        List<Functionality> newFilter = new List<Functionality>();

        foreach (ItemTypeContainer.SetItemType i in functionalityWhiteList)
        {
            newFilter.AddRange(ItemTypeContainer.ItemTypeDictionary[i].Functionalities);
        }

        return newFilter.ToArray();
    }

}
