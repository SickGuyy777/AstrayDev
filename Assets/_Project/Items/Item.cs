using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    private static int LastInstanceID;
    private int instanceID = -1;
    
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
                this.Components =  null;
                amount = 0;
                instanceID = -1;
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
                this.Components = null;
                info = null;
                instanceID = -1;
            }
            
            amount = value;
        }
    }

    public string Name => Info.name;
    public int MaxStack => Info.maxStack;
    public Sprite Icon => Info?.itemIcon;
    public ItemObject Prefab => Info?.itemPrefab;
    public bool IsEmpty => Amount <= 0;
    public bool IsFull => amount >= info.maxStack;
    public List<ItemComponent> Components { get; private set; } = new List<ItemComponent>();


    public Item(ItemInfo info, int amount, ItemComponent[] components = null, int id = -1)
    {
        instanceID = id  <= -1 ? LastInstanceID + 1 : id;
        LastInstanceID = instanceID;
        
        AttachComponents(components);

        this.info = info;
        this.amount = amount;
    }
    
    public Item Clone() => new Item(info, amount, Components?.ToArray(), instanceID);
    
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
        this.Components = itemToCopy.Components;
        this.instanceID = itemToCopy.instanceID;
    }

    public Item Transfer(int transferAmount = 0)
    {
        Item copy = Clone();

        transferAmount = transferAmount <= 0 ? Amount : transferAmount;
        
        Amount -= transferAmount;
        copy.Amount = transferAmount;

        return copy;
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

    private bool IsNullOrThis(Item item) => item == null || item == this;

    public override bool Equals(object obj) => (Item)obj == this;

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(Item a, Item b)
    {
        if (a is null && b is null)
            return true;

        if (a is null && !(b is null) || !(a is null) && b is null)
            return false;

        return a.instanceID == b.instanceID;
    }

    public static bool operator !=(Item a, Item b)
    {
        if (a is null && b is null)
            return false;

        if (a is null && !(b is null) || !(a is null) && b is null)
            return true;

        return a.instanceID != b.instanceID;
    }
}
