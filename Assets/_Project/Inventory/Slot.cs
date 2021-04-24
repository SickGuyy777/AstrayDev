using UnityEngine.Rendering;

public class Slot
{
    public Item Item { get; private set; }
    public ItemInfo Info { get; private set; }
    public int Amount => Item.Amount;
    public int MaxStack => Item.MaxStack;
    public bool IsEmpty => Item == null || Amount == 0;
    public bool IsFull => Item != null && Amount == MaxStack;

    public Slot() => SetItem(new Item(null, 0));

    /// <summary>
    /// Add <paramref name="itemToAdd"/> to <see langword="this"/> <see cref="Slot"/>.
    /// </summary>
    /// <returns>The remaining amount.</returns>
    public int AddItem(Item itemToAdd, int amountToAdd = 0)
    {
        if (IsEmpty)
            Item.CopyAttributes(itemToAdd);

        if (!Item.IsSameItemType(itemToAdd.Info))
            return itemToAdd.Amount;

        if (amountToAdd <= 0)
            amountToAdd = itemToAdd.Amount;

        int remaining = amountToAdd + Item.Amount - Item.MaxStack;

        if (amountToAdd > itemToAdd.Amount)
            amountToAdd = itemToAdd.Amount;

        if (amountToAdd + Item.Amount > Item.MaxStack)
            amountToAdd = Item.MaxStack - Item.Amount;

        itemToAdd.Amount -= amountToAdd;
        Item.Amount += amountToAdd;

        return remaining;
    }

    public bool CanAdd(ItemInfo info) => IsEmpty || !IsFull && Item.IsSameItemType(info);

    public bool IsSameItemType(ItemInfo info) => Item.IsSameItemType(info);

    public void SetItem(Item item)
    {
        Item = item;

        if (item == null)
        {
            Info = null;
            return;
        }

        Info = item.Info;
    }
}