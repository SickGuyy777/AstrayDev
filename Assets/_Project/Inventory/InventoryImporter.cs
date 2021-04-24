using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class InventoryImporter : MonoBehaviour
{
    [SerializeField] private SerializableItem[] itemInfosToAdd;

    private Inventory inventory;


    private void Awake() => inventory = GetComponent<Inventory>();

    private void Start() => CreateItems();

    private void CreateItems()
    {
        inventory.AddToInventory(InfoToItems());
        Destroy(this);
    }

    private Item[] InfoToItems()
    {
        List<Item> items = new List<Item>();
        
        foreach (SerializableItem serializableItem in itemInfosToAdd)
        {
            Item item = serializableItem.info.GetNewItem();
            item.Amount = serializableItem.amount;
            
            items.Add(item);
        }

        return items.ToArray();
    }
}

[System.Serializable]
public struct SerializableItem
{
    public ItemInfo info;
    public int amount;
}
