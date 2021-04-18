using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCursor : MonoBehaviour
{
    public static PlayerCursor Instance;
    
    [SerializeField] private ItemDisplaySlot grabDisplay;
    
    public Item HoldingItem { get; private set; } = new Item(null, 0);

    private bool dragging => !HoldingItem.IsEmpty;
    private ItemDisplaySlot lastItemSlot;


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
        ItemDisplaySlot hoveringDisplaySlot = ItemDragable.currentSelectedDragable?.DisplaySlot;
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
        
        if (Input.GetMouseButtonDown(1))
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
    
    private void GrabItem(ItemDisplaySlot slot, int grabAmount = -1)
    {
        lastItemSlot = slot;
        
        Item item = slot.CurrentItem;
        if(item.IsEmpty)
            return;
        
        Item newItem = item.Transfer(grabAmount);

        HoldingItem = newItem;
        grabDisplay.SetItemReference(HoldingItem);
    }

    private void PlaceItem(ItemDisplaySlot slot, int placeAmount = -1)
    {
        bool empty = slot.CurrentItem.IsEmpty;
        bool sameItem = slot.CurrentItem.IsSameItem(HoldingItem);
        
        Inventory inventory = slot.Inventory;
        if(!inventory.CanAdd(HoldingItem.Clone().Transfer()))
            return;
        
        if(empty || sameItem && !slot.CurrentItem.IsFull)
            AddItem(slot, placeAmount);
        else
            SwapWithHand(slot);
    }
    
    private void AddItem(ItemDisplaySlot slot, int addAmount)
    {
        bool empty = slot.CurrentItem.IsEmpty;
        bool sameItem = slot.CurrentItem.IsSameItem(HoldingItem);
        
        if(!empty && !sameItem)
            return;
        
        if(HoldingItem == null) 
            return;

        Inventory inventory = slot.Inventory;
        
        if(!inventory.CanAdd(HoldingItem.Clone().Transfer()))
            return;
        
        inventory.AddToInventory(HoldingItem.Transfer(addAmount), slot.SlotIndex);
        lastItemSlot = null;
        grabDisplay.SetItemReference(HoldingItem);
    }
    
    private void SwapWithHand(ItemDisplaySlot slot)
    {
        if(HoldingItem == null) 
            return;

        Inventory inventory = slot.Inventory;
        
        if(!inventory.CanAdd(HoldingItem.Clone().Transfer()))
            return;
        
        Item slotItem = slot.CurrentItem.Transfer();
        
        inventory.AddToInventory(HoldingItem.Transfer(), slot.SlotIndex);
        HoldingItem = slotItem;
        
        grabDisplay.SetItemReference(HoldingItem);
    }

    public void Drop(int amount = -1)
    {
        if(lastItemSlot == null)
            return;
        
        HoldingItem?.Drop(lastItemSlot.Inventory.transform.position, amount);
    }
}
