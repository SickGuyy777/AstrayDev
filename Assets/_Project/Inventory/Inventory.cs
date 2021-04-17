using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Item[] items = new Item[30];
    public Item[] Items => items;


    public void AddToInventory(Item itemToAdd, int slotIndex = -1)
    {
        if(itemToAdd.Info == null)
            return;
        
        while(itemToAdd.Amount > 0)
        {
            int slot = GetBestSlot(itemToAdd.Info);
            
            if (slotIndex != -1)
            {
                Item item = items[slotIndex];
                if (IsSameItem(itemToAdd.Info, item.Info) && !item.IsFull)
                    slot = slotIndex;
                if (item.IsEmpty)
                    slot = slotIndex;
            }

            if (slot == -1)
                break;
            
            Item itemSlot = items[slot];
            
            itemSlot.Info = itemToAdd.Info;
            itemSlot.Amount++;
            itemToAdd.Amount--;
        }
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
        for (int i = 0; i < items.Length; i++)
        {
            Item item = items[i];

            bool sameItemSlot = IsSameItem(item.Info, info) && !IsFull(i);

            if (sameItemSlot)
                return i;
        }
        
        for (int i = 0; i < items.Length; i++)
        {
            if (IsEmpty(i))
                return i;
        }

        return -1;
    }
    
    private bool IsSameItem(ItemInfo info1, ItemInfo info2) => info1 == info2;

    private bool IsFull(int slotIndex) => items[slotIndex].Amount >= items[slotIndex].MaxStack;

    private bool IsEmpty(int slotIndex) => items[slotIndex].Amount <= 0;
}
