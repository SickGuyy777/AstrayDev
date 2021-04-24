
[System.Serializable]
public class Slot
{
    private Item item;
    public Item Item
    {
        get => item; 
        
        set
        {
            item = value;

            if (value == null)
            {
                Info = null;
                return;
            }

            Info = item.Info;
        }
    }

    public ItemInfo Info { get; private set; }
    public int Amount => Item.Amount;
    public int MaxStack => Info.maxStack;
    public bool IsEmpty => Item == null || Amount == 0;
    public bool IsFull => Item != null && Amount == MaxStack;


    public Slot(Item item = null)
    {
        Item itemToSet = item ?? new Item(null, 0);
        SetItem(itemToSet);
    }

    public bool CanAdd(ItemInfo info) => !IsFull && Item.IsSameItemType(info) || IsEmpty;
    
    /// <summary>
    /// Add <paramref name="itemToAdd"/> to <see langword="this"/> <see cref="Slot"/>.
    /// </summary>
    /// <returns>The remaining amount.</returns>
    public int AddItem(Item itemToAdd, int amountToAdd = 0)
    {
        bool isSameItem = !IsFull && Item.IsSameItemType(itemToAdd.Info);
        
        
        if (!isSameItem && !IsEmpty)
            return itemToAdd.Amount;

        int remaining = 0;
        
        if (amountToAdd <= 0)
            amountToAdd = itemToAdd.Amount;
        
        if (amountToAdd > itemToAdd.Amount)
            amountToAdd = itemToAdd.Amount;
        
        if (!IsEmpty)
        {
            remaining = amountToAdd + Item.Amount - Item.MaxStack;

            if (amountToAdd + Item.Amount > Item.MaxStack)
                amountToAdd = Item.MaxStack - Item.Amount;
        }
        else
            Item.CopyAttributes(itemToAdd);
        
        itemToAdd.Amount -= amountToAdd;
        Item.Amount += amountToAdd;

        return remaining;
    }

    public bool IsSameItemType(ItemInfo info) => Item.IsSameItemType(info);

    public void SetItem(Item item) => Item = item;
}    

