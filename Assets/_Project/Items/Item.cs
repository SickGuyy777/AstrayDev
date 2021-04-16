using System;
using UnityEngine;
using Random = System.Random;

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


    public Item(ItemInfo info, int amount)
    {
        this.info = info;
        this.amount = amount;
    }
    
    public Item Copy() => new Item(Info, Amount);

    public Item Transfer()
    {
        Item copy = Copy();
        Amount = 0;
        
        return copy;
    }

    public bool IsSameItem(Item item) => item.info == info;
    
    public bool IsSameItem(ItemInfo itemInfo) => itemInfo == info;

    public void Drop(Vector2 position)
    {
        for (int i = 0; i < Amount; i++)
        {
            Debug.Log(Amount);
            float x = UnityEngine.Random.Range(-.5f, .5f);
            float y = UnityEngine.Random.Range(-.5f, .5f);
            Vector2 randOffset = new Vector2(x, y);
            
            ItemObject itemObject = GameObject.Instantiate(Prefab, position + randOffset, Quaternion.identity, null);
        }
        
        Amount = 0;
    }
}
