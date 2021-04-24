using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class ItemDisplaySlot : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;

    public Slot CurrentSlot { get; private set; }
    public Inventory Inventory => inventory;

    public int SlotIndex { get; private set; } = 0;


    private void Start() => CurrentSlot ??= inventory?.Slots[SlotIndex];

    public void SetSlotReference(Slot slot) => this.CurrentSlot = slot;

    public void Setup(Inventory aInventory, int slotIndex)
    {
        this.inventory = aInventory;
        SlotIndex = slotIndex;
    }

    private void Update()
    {
        if (CurrentSlot != null)
        {
            icon.sprite = CurrentSlot.Item.Icon;
            icon.color = !CurrentSlot.IsEmpty ? Color.white : Color.clear;

            bool showAmountText = CurrentSlot.Amount > 1 && !CurrentSlot.IsEmpty;
            
            amountText.text = showAmountText? CurrentSlot.Amount.ToString("0") : "";
        }
    }
}
