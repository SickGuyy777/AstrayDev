using Assets._Project.Inventory;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Slot[] slots = new Slot[30];

    public Slot[] Slots => slots;
    private InventoryFilter filter;
    public System.Action OnChanged;

    private void Awake()
    {
        filter = GetComponent<InventoryFilter>();
    }

    public bool CanAdd(Item itemToAdd) => filter == null ||  filter != null && filter.ContainedInWhitelist(itemToAdd);

    public bool AddToInventory(Item itemToAdd, int slotIndex = -1, int amountToAdd = 0)
    {
        if (amountToAdd <= 0 || amountToAdd > itemToAdd.Amount)
            amountToAdd = itemToAdd.Amount;

        int endAmount = itemToAdd.Amount - amountToAdd;

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
                Item item = Slots[slotIndex].Item;
                if (IsSameItem(itemToAdd.Info, item.Info) && !item.IsFull)
                    bestSlotIndex = slotIndex;
                if (item.IsEmpty)
                    bestSlotIndex = slotIndex;
            }

            if (bestSlotIndex == -1)
                break;
            
            Slots[bestSlotIndex].Item.Add(itemToAdd, 1);
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
        for (int i = 0; i < Slots.Length; i++)
        {
            Item item = Slots[i].Item;

            bool sameItemSlot = IsSameItem(item?.Info, info) && !IsSlotFull(i);

            if (sameItemSlot)
                return i;
        }
        
        for (int i = 0; i < Slots.Length; i++)
        {
            if (IsEmpty(i))
                return i;
        }

        return -1;
    }
    
    private bool IsSameItem(ItemInfo info1, ItemInfo info2) => info1 == info2;

    private bool IsSlotFull(int slotIndex) => Slots[slotIndex].Amount == Slots[slotIndex].MaxStack;

    private bool IsEmpty(int slotIndex) => Slots[slotIndex]?.Amount <= 0;
}
