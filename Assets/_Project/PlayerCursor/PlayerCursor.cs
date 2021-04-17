using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCursor : MonoBehaviour
{
    public static PlayerCursor Instance;
    
    [SerializeField] private InventoryDisplaySlot grabDisplay;
    
    public Item HoldingItem { get; private set; } = new Item(null, 0);

    private bool dragging => !HoldingItem.IsEmpty;
    private InventoryDisplaySlot lastInventorySlot;


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

        UpdateDragging();
    }

    private void UpdateDragging()
    {
        InventoryDisplaySlot hoveringDisplaySlot = ItemDragable.currentSelectedDragable?.DisplaySlot;
        bool selectedASlot = hoveringDisplaySlot != null;
        bool hoveringOverUI = EventSystem.current.IsPointerOverGameObject();
        
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedASlot)
            {
                if (!dragging)
                    GrabItem(hoveringDisplaySlot);
                else
                    PlaceItem(hoveringDisplaySlot);
            }
            else if (!hoveringOverUI)
                Drop();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (selectedASlot)
            {
                int half = Mathf.CeilToInt(hoveringDisplaySlot.CurrentItem.Amount / 2f);
                if (!dragging)
                    GrabItem(hoveringDisplaySlot, half);
                else
                    AddItem(hoveringDisplaySlot, 1);
            }
            else if (!hoveringOverUI)
                Drop(1);
        }
    }
    
    private void GrabItem(InventoryDisplaySlot slot, int grabAmount = -1)
    {
        lastInventorySlot = slot;
        
        Item item = slot.CurrentItem;
        Item newItem = item.Transfer(grabAmount);
        
        HoldingItem = newItem;
        grabDisplay.SetItemReference(HoldingItem);
    }

    private void PlaceItem(InventoryDisplaySlot slot, int placeAmount = -1)
    {
        bool empty = slot.CurrentItem.IsEmpty;
        bool sameItem = slot.CurrentItem.IsSameItem(HoldingItem);
        
        if(empty || sameItem && !slot.CurrentItem.IsFull)
            AddItem(slot, placeAmount);
        else
            SwapWithHand(slot);
    }
    
    private void AddItem(InventoryDisplaySlot slot, int addAmount)
    {
        bool empty = slot.CurrentItem.IsEmpty;
        bool sameItem = slot.CurrentItem.IsSameItem(HoldingItem);
        
        if(!empty && !sameItem)
            return;
        
        if(HoldingItem == null) 
            return;

        Inventory inventory = slot.Inventory;
        Item newItem = HoldingItem.Transfer(addAmount);
        inventory.AddToInventory(newItem, slot.SlotIndex);
        lastInventorySlot = null;
        grabDisplay.SetItemReference(HoldingItem);
    }
    
    private void SwapWithHand(InventoryDisplaySlot slot)
    {
        if(HoldingItem == null) 
            return;

        Inventory inventory = slot.Inventory;
        Item handItem = HoldingItem.Transfer();
        Item slotItem = slot.CurrentItem.Transfer();
        
        inventory.AddToInventory(handItem, slot.SlotIndex);
        HoldingItem = slotItem;
        
        grabDisplay.SetItemReference(HoldingItem);
    }

    public void Drop(int amount = -1)
    {
        if(lastInventorySlot == null)
            return;
        
        HoldingItem?.Drop(lastInventorySlot.Inventory.transform.position, amount);
    }

    
}
