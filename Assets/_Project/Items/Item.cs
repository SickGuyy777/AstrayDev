using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Item
{
    private ItemInfo info;
    public ItemInfo Info
    {
        get => info;

        set
        {
            if(value == info)
                return;
            
            if (value == null)
            {
                this.Components.Clear();
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
            if(value == amount)
                return;
            
            if (value <= 0)
            {
                this.Components.Clear();
                info = null;
            }
            
            amount = value;
        }
    }

    public string Name => Info.name;
    public int MaxStack => Info.maxStack;
    public Sprite Icon => Info?.itemIcon;
    public ItemPickup Prefab => Info?.itemPrefab;
    public bool IsEmpty => Amount <= 0;
    public bool IsFull => amount >= info.maxStack;
    public List<ItemComponent> Components { get; private set; }


    public Item(ItemInfo info, int amount, List<ItemComponent> components = null)
    {
        Components = new List<ItemComponent>();

        if(components != null)
            CopyComponents(components);

        this.info = info;
        this.amount = amount;
    }

    public Item Clone()
    {
        Item item = new Item(info, amount);
        item.CopyAttributes(this);

        return item;
    }

    public void Copy(Item itemToCopy)
    {
        if(IsNullOrThis(itemToCopy))
            return;
        
        this.Amount = itemToCopy.Amount;
        
        CopyAttributes(itemToCopy);
    }
    
    public void CopyAttributes(Item itemToCopy)
    {
        if(IsNullOrThis(itemToCopy))
            return;
        
        this.Info = itemToCopy.Info;
        CopyComponents(itemToCopy.Components);
    }

    public Item Transfer(int transferAmount = 0)
    {
        Item copy = Clone();

        transferAmount = transferAmount <= 0 ? Amount : transferAmount;
        
        Amount -= transferAmount;
        copy.Amount = transferAmount;

        return copy;
    }
    
    public int TransferAmount(int transferAmount = 0)
    {
        transferAmount = transferAmount <= 0 ? Amount : transferAmount;
        Amount -= transferAmount;

        return transferAmount;
    }

    public void Add(Item itemToAdd, int transferAmount)
    {
        CopyAttributes(itemToAdd);
        
        Amount += transferAmount;
        itemToAdd.Amount -= transferAmount;
    }

    public bool IsSameItemType(ItemInfo itemInfo) => itemInfo == info;

    public void Drop(Vector2 position, int dropAmount = 0)
    {
        if(IsEmpty)
            return;
        
        dropAmount = dropAmount <= 0 ? Amount : amount;
        
        float x = Random.Range(-.5f, .5f);
        float y = Random.Range(-.5f, .5f);
        Vector2 randOffset = new Vector2(x, y);
            
        ItemPickup createdPickup = GameObject.Instantiate(Prefab, position + randOffset, Quaternion.identity, null);
        createdPickup.Item.Amount = dropAmount;

        Amount -= dropAmount;
    }

    public T GetComponent<T>() where T : ItemComponent
    {
        if (Components == null)
            return null;
        
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

    public void CopyComponents(List<ItemComponent> components)
    {
        foreach (ItemComponent itemComponent in components)
        {
            AttachComponent(itemComponent.Copy());
        }
    }

    public void AttachComponent(ItemComponent component)
    {
        if(component == null)
            return;
        
        Components ??= new List<ItemComponent>();
        
        Components.Add(component);
        component.SetItem(this);
    }
    
    public void AttachComponents(List<ItemComponent> components)
    {
        if(components == null)
            return;
        
        foreach (ItemComponent component in components)
        {
            AttachComponent(component);
        }
    }

    private bool IsNullOrThis(Item item) => item == null || item == this;
}
