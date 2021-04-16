using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Item[] items = new Item[30];
    public Item[] Items => items;


    public void AddToInventory(Item itemToAdd, int slotToAdd = -1)
    {
        if(itemToAdd.Info == null)
            return;
        
        while(itemToAdd.Amount > 0)
        {
            int slot = GetBestSlot(itemToAdd.Info);
            
            if (slotToAdd != -1)
            {
                Item item = items[slotToAdd];
                if (IsSameItem(itemToAdd.Info, item.Info) && !item.IsFull)
                    slot = slotToAdd;
                if (item.IsEmpty)
                    slot = slotToAdd;
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

    private bool IsFull(int slot) => items[slot].Amount >= items[slot].MaxStack;

    private bool IsEmpty(int slot) => items[slot].Amount <= 0;
}
