using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class InventoryDisplaySlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;

    private Color textColor;
    private Item itemSlot;
    private Inventory inventory;

    public Inventory Inventory => inventory;
    public Item ItemSlot => itemSlot;
    public int Slot { get; private set; }


    private void Awake()
    {
        textColor = amountText.color;
    }

    public void SetItem(Item newItem) => this.itemSlot = newItem;

    public void SetInventory(Inventory aInventory) => this.inventory = aInventory;
    
    public void SetIndex(int slotIndex) => this.Slot = slotIndex;

    private void Update()
    {
        if (itemSlot != null)
        {
            icon.sprite = itemSlot.Icon;
            icon.color = !itemSlot.IsEmpty ? Color.white : Color.clear;
        
            amountText.color = itemSlot.IsEmpty != null && itemSlot.Amount > 1 ? textColor : Color.clear;
            amountText.text = itemSlot.IsEmpty != null && itemSlot.Amount > 1 ? itemSlot.Amount.ToString("0") : "";
        }
    }
}
