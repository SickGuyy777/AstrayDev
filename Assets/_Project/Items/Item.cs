using System;
using System.Collections.Generic;
using System.Linq;
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
            {
                this.Components =  null;
                amount = 0;
            }
            
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
            {
                this.Components = null;
                info = null;
            }

            amount = value;
        }
    }

    public string Name => Info.name;
    public int MaxStack => Info.maxStack;
    public Sprite Icon => Info?.itemIcon;
    public ItemObject Prefab => Info?.itemPrefab;
    public bool IsEmpty => Info == null;
    public bool IsFull => amount >= info.maxStack;
    public List<ItemComponent> Components { get; private set; }


    public Item(ItemInfo info, int amount, ItemComponent[] components = null)
    {
        Components = new List<ItemComponent>();
        AttachComponents(components);

        this.info = info;
        this.amount = amount;
    }
    
    public Item Clone() => new Item(info, amount, Components.ToArray());
    
    public void Copy(Item itemToCopy)
    {
        this.Amount = itemToCopy.Amount;
        this.Info = itemToCopy.Info;
        this.Components = itemToCopy.Components;
    }
    
    public void CopyInfo(Item itemToCopy)
    {
        this.Info = itemToCopy.Info;
        this.Components = itemToCopy.Components;
    }

    public Item Transfer(int transferAmount = -1)
    {
        Item copy = Clone();

        if (transferAmount <= -1)
            transferAmount = Amount;
        
        Amount -= transferAmount;
        copy.Amount = transferAmount;

        return copy;
    }

    public void Add(Item itemToAdd, int transferAmount)
    {
        CopyInfo(itemToAdd);
        
        Amount += transferAmount;
        itemToAdd.Amount -= transferAmount;
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

    public T GetComponent<T>() where T : ItemComponent
    {
        foreach (ItemComponent component in Components)
        {
            if (component is T t)
                return t;
        }

        return null;
    }
    
    public T AddComponent<T>() where T : ItemComponent, new()
    {
        T createdComponent = new T();
        
        AttachComponent(createdComponent);
        
        return createdComponent;
    }

    public void AttachComponent(ItemComponent component)
    {
        if(component == null)
            return;
        
        Components.Add(component);
        component.SetItem(this);
    }
    
    public void AttachComponents(ItemComponent[] components)
    {
        if(components == null)
            return;
        
        foreach (ItemComponent component in components)
        {
            AttachComponent(component);
        }
    }
}
