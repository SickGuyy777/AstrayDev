using System;
using UnityEngine;

[Serializable]
public class Item
{
    private ItemInfo info;
    public ItemInfo Info
    {
        get => info;

        set
        {
            if (value == null)
                amount = 0;

            info = value;
        }
    }

    private int amount;
    public int Amount
    {
        get => amount;

        set
        {
            if (value <= 0)
                info = null;
            
            amount = value;
        }
    }

    public string Name => Info.name;
    public int MaxStack => Info.maxStack;
    public Sprite Icon => Info?.itemIcon;
    public ItemObject Prefab => Info?.itemPrefab;
    public bool IsEmpty => Info == null;
    public bool IsFull => amount >= info.maxStack;
    public ItemType ItemType { get; private set; }


    public Item(ItemInfo info, int amount, ItemType itemType = null)
    {
        this.ItemType = itemType ?? ItemTypeContainer.None;
        
        this.info = info;
        this.amount = amount;
    }
    
    public Item Copy() => new Item(Info, Amount);

    public Item Transfer(int transferAmount = -1)
    {
        Item copy = Copy();
        
        if (transferAmount <= -1)
            transferAmount = Amount;
        
        Amount -= transferAmount;
        copy.Amount = transferAmount;
        
        return copy;
    }

    public bool IsSameItem(Item item) => item.info == info;
    
    public bool IsSameItem(ItemInfo itemInfo) => itemInfo == info;

    public void Drop(Vector2 position, int dropAmount = -1)
    {
        if(IsEmpty)
            return;
        
        if (dropAmount <= -1)
            dropAmount = Amount;
        
        for (int i = 0; i < dropAmount; i++)
        {
            float x = UnityEngine.Random.Range(-.5f, .5f);
            float y = UnityEngine.Random.Range(-.5f, .5f);
            Vector2 randOffset = new Vector2(x, y);
            
            ItemObject itemObject = GameObject.Instantiate(Prefab, position + randOffset, Quaternion.identity, null);
        }

        Amount -= dropAmount;
    }

    public T GetFunctionality<T>() where T : Functionality
    {
        foreach (Functionality functionality in ItemType.Functionalities)
        {
            if (functionality is T t)
                return t;
        }

        return null;
    }
}
