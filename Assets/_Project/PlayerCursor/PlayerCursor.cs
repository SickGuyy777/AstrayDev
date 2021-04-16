using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    [SerializeField] private InventoryDisplaySlot displaySlot;
    
    public static PlayerCursor Instance;

    private InventoryDisplaySlot lastInventorySlot;
    public Item HoldingItem => holdingItem;
    private Item holdingItem = new Item(null, 0);


    private void Awake()
    {
        #region Singleton

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        
        #endregion
    }

    private void Update()
    {
        transform.position = Input.mousePosition;

        displaySlot.SetItem(holdingItem);
    }
    

    public void GrabItem(InventoryDisplaySlot slot)
    {
        lastInventorySlot = slot;
        
        Item item = slot.ItemSlot;
        Item newItem = item.Transfer();
        
        holdingItem = newItem;
    }

    public void PlaceItem(InventoryDisplaySlot slot)
    {
        if(holdingItem == null) 
            return;

        Inventory inventory = slot.Inventory;
        Item newItem = holdingItem.Transfer();
        inventory.AddToInventory(newItem, slot.Slot);
        lastInventorySlot = null;
    }
    
    public void SwapItem(InventoryDisplaySlot slot)
    {
        if(holdingItem == null) 
            return;

        Inventory inventory = slot.Inventory;
        Item handItem = holdingItem.Transfer();

        inventory.AddToInventory(slot.ItemSlot, lastInventorySlot.Slot);
        inventory.AddToInventory(handItem, slot.Slot);
    }
}
