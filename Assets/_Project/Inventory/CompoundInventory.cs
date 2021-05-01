using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class CompoundInventory : MonoBehaviour
{
    [SerializeField] private Inventory[] inventories;
    
    public Inventory CompoundedInventory { get; private set; }

    private void Start()
    {
        CompoundedInventory = GetComponent<Inventory>();

        foreach (Inventory inventory in inventories)
        {
            inventory.SetCompound(this);
            inventory.OnChanged += UpdateCompoundInventory;
        }
        
        UpdateCompoundInventory();
    }

    private void UpdateCompoundInventory()
    {
        CompoundedInventory.Set(GetInventoryLength());
        
        foreach (Inventory inventory in inventories)
        {
            foreach (Item item in inventory.GetItems())
            {
                CompoundedInventory.AddToInventory(item.Clone());
            }
        }
    }

    private int GetInventoryLength() => inventories.Sum(inventory => inventory.Slots.Length);
}
