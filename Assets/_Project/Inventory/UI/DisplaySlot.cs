using UnityEngine;


public class DisplaySlot : MonoBehaviour
{
    public Inventory Inventory { get; private set; }
    public Slot CurrentSlot { get; private set; }

    private ItemDisplaySlot currentItemDisplaySlot;
    private RectTransform rectTransform;


    private void Awake()
    {
        if(rectTransform == null)
            this.rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(Inventory inventory, Slot slot)
    {
        if(rectTransform == null)
            this.rectTransform = GetComponent<RectTransform>();

        this.Inventory = inventory;
        SetSlotReference(slot);
    }

    private void UpdateItemSlotDisplay()
    {
        if (currentItemDisplaySlot != null)
            Destroy(currentItemDisplaySlot.gameObject);
        
        if(CurrentSlot == null || CurrentSlot.IsEmpty)
            return;
        
        ItemDisplaySlot createdSlot = Instantiate(CurrentSlot.Item.Info.displaySlot, rectTransform.position, rectTransform.rotation, rectTransform);
        createdSlot.Setup(Inventory, CurrentSlot);

        currentItemDisplaySlot = createdSlot;
        createdSlot.RectTransform.sizeDelta = rectTransform.sizeDelta;
    }

    public void SetSlotReference(Slot slot)
    {
        if (CurrentSlot != null)
            CurrentSlot.Item.OnChanged -= UpdateItemSlotDisplay;
        
        slot.Item.OnChanged += UpdateItemSlotDisplay;

        CurrentSlot = slot;
        
        UpdateItemSlotDisplay();
    }
}
