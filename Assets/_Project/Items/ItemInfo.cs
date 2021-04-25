using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemInfo : ScriptableObject
{
    public string itemName = "New Item";
    public int maxStack = 16;
    public Sprite itemIcon;
    public ItemObject itemPrefab;


    protected abstract List<ItemComponent> GetComponents();

    public Item GetNewItem() => new Item(this, 1, GetComponents());
}
