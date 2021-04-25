using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryCursor : MonoBehaviour
{
    public static InventoryCursor Instance;
    [SerializeField] private ItemDisplaySlot grabDisplay;
    
    public Slot HoldingSlot { get; private set; } = new Slot();

    private bool Dragging => !HoldingSlot.IsEmpty;
    private ItemDisplaySlot lastItemSlot;
    private RectTransform rectTransform;


    private void Awake()
    {
        #region Singleton

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        
        #endregion
        
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update() => UpdateDragging();

    private void FixedUpdate() => rectTransform.position = Input.mousePosition;

    private void UpdateDragging()
    {
        ItemDisplaySlot hoveringDisplaySlot = ItemDragable.currentSelectedDragable?.DisplaySlot;
        bool selectedASlot = hoveringDisplaySlot != null;
        bool hoveringOverUI = EventSystem.current.IsPointerOverGameObject() || selectedASlot;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (!hoveringOverUI)
            {
                Drop();
                return;
            }
            
            if (selectedASlot)
            {
                if (!Dragging)
                    GrabItem(hoveringDisplaySlot);
                else
                    PlaceItem(hoveringDisplaySlot);
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            if (!hoveringOverUI)
            {
                Drop(1);
                return;
            }
            
            if (selectedASlot)
            {
                int half = Mathf.CeilToInt(hoveringDisplaySlot.CurrentSlot.Amount / 2f);
                
                if (!Dragging)
                    GrabItem(hoveringDisplaySlot, half);
                else
                    AddItem(hoveringDisplaySlot, 1);
            }
        }
    }
    
    private void GrabItem(ItemDisplaySlot displaySlot, int grabAmount = 0)
    {
        lastItemSlot = displaySlot;
        
        Slot newSlot = displaySlot.CurrentSlot;
        Item item = newSlot.Item;

        if(item.IsEmpty)
            return;

        Inventory inventory = displaySlot.Inventory;

        HoldingSlot.SetItem(item.Transfer(grabAmount));
        grabDisplay.SetSlotReference(HoldingSlot);
        
        inventory.OnChanged?.Invoke();
    }

    private void PlaceItem(ItemDisplaySlot slot, int placeAmount = -1)
    {
        bool empty = slot.CurrentSlot.IsEmpty;
        bool sameItem = slot.CurrentSlot.IsSameItemType(HoldingSlot.Item.Info);
        
        Inventory inventory = slot.Inventory;
        
        if(!inventory.IsItemTypeAllowed(HoldingSlot.Item.Clone().Transfer()))
            return;
        
        if(empty || sameItem && !slot.CurrentSlot.IsFull)
            AddItem(slot, placeAmount);
        else
            SwapWithHand(slot);
    }
    
    private void AddItem(ItemDisplaySlot slot, int addAmount)
    {
        bool empty = slot.CurrentSlot.IsEmpty;
        bool sameItem = slot.CurrentSlot.IsSameItemType(HoldingSlot.Item.Info);
        
        if(!empty && !sameItem)
            return;
        
        if(HoldingSlot == null) 
            return;

        Inventory inventory = slot.Inventory;

        bool canAdd = inventory.IsItemTypeAllowed(HoldingSlot.Item) && !inventory.IsFullForItemType(HoldingSlot.Item.Info);
                      
        if(!canAdd)
            return;
        
        inventory.AddToInventory(HoldingSlot.Item, slot.CurrentSlot, addAmount);
        lastItemSlot = slot;
        grabDisplay.SetSlotReference(HoldingSlot);
    }
    
    private void SwapWithHand(ItemDisplaySlot displaySlot)
    {
        if(HoldingSlot == null) 
            return;
        
        Inventory inventory = displaySlot.Inventory;
        
        if(!inventory.IsItemTypeAllowed(HoldingSlot.Item))
            return;

        Item placingItem = displaySlot.CurrentSlot.Item.Transfer();
        Item handItem = HoldingSlot.Item.Transfer();

        inventory.AddToInventory(handItem, displaySlot.CurrentSlot);
        HoldingSlot.SetItem(placingItem.Transfer());

        grabDisplay.SetSlotReference(HoldingSlot);
    }

    public void Drop(int amount = -1)
    {
        if(lastItemSlot == null)
            return;
        
        HoldingSlot?.Item?.Drop(lastItemSlot.Inventory.transform.position, amount);
    }
}
