using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class InventoryDisplaySlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;

    public Item CurrentItem { get; private set; }
    public Inventory Inventory { get; private set; }
    
    public int SlotIndex { get; private set; }

    
    public void SetItemReference(Item newItem) => this.CurrentItem = newItem;

    public void Setup(Inventory aInventory, int slotIndex)
    {
        this.Inventory = aInventory;
        SlotIndex = slotIndex;
    }

    private void Update()
    {
        if (CurrentItem != null)
        {
            icon.sprite = CurrentItem.Icon;
            icon.color = !CurrentItem.IsEmpty ? Color.white : Color.clear;

            bool showAmountText = CurrentItem.Amount > 1 && !CurrentItem.IsEmpty;
            
            amountText.text = showAmountText? CurrentItem.Amount.ToString("0") : "";
        }
    }
}
