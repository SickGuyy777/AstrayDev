
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public abstract class ItemDisplaySlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;

    protected Item itemReference;
    protected Inventory inventory;
    
    private RectTransform rectTransform;
    public RectTransform RectTransform => rectTransform;
    

    public void Setup(Inventory aInventory, Slot slot)
    {
        rectTransform = GetComponent<RectTransform>();

        this.inventory = aInventory;
        itemReference = slot.Item;
        
        UpdateDisplay();
    }
    
    protected virtual void UpdateDisplay()
    {
        icon.sprite = itemReference.Icon;
        icon.color = !itemReference.IsEmpty ? Color.white : Color.clear;

        bool showAmountText = itemReference.Amount > 1 && !itemReference.IsEmpty;
            
        amountText.text = showAmountText? itemReference.Amount.ToString("0") : "";
    }
}
