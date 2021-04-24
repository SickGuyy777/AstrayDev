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

        foreach (Slot slot in slots)
        {
            slot.SetItem(new Item(null, 0));
        }
    }

    public bool IsItemTypeAllowed(Item itemToAdd) => filter == null ||  filter != null && filter.ContainedInWhitelist(itemToAdd);

    public bool IsFullForItemType(ItemInfo info) => GetBestSlot(info) == null;

    public void AddToInventory(Item itemToAdd, Slot startingSlot = null, int amountToAdd = 0)
    {
        if (!IsItemTypeAllowed(itemToAdd))
            return;
        
        if (amountToAdd <= 0 || amountToAdd > itemToAdd.Amount)
            amountToAdd = itemToAdd.Amount;
        
        int i = 0;
        while (amountToAdd > 0)
        {
            if(IsFullForItemType(itemToAdd.Info))
                return;
            
            Debug.Log("looped x" + i + " times");
            i++;
            
            if (startingSlot != null && startingSlot.CanAdd(itemToAdd.Info))
                amountToAdd = startingSlot.AddItem(itemToAdd, amountToAdd);
            else if(startingSlot == null)
                amountToAdd = GetBestSlot(itemToAdd.Info).AddItem(itemToAdd, amountToAdd);
            
            if (i >= 100)
            {
                Debug.LogError("While loop will go on forever");
                break;
            }
        }
        
        OnChanged?.Invoke();
    }

    public void AddToInventory(Item[] itemsToAdd)
    {
        foreach (Item item in itemsToAdd)
        {
            AddToInventory(item);
        }
    }

    private Slot GetBestSlot(ItemInfo info)
    {
        foreach (Slot slot in Slots)
        {
            bool sameItemSlot = slot.IsSameItemType(info) && !slot.IsFull;

            if (sameItemSlot)
                return slot;
        }

        foreach (Slot slot in Slots)
        {
            if (slot.IsEmpty)
                return slot;
        }

        return null;
    }
}
