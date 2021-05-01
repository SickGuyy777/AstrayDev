using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private Inventory inventoryReference;
    [SerializeField] private GridLayoutGroup group;
    [SerializeField] private DisplaySlot displaySlotPrefab;

    
    private void Start()
    {
        foreach (Slot slot in inventoryReference.Slots)
        {
            DisplaySlot displaySlot = Instantiate(displaySlotPrefab, Vector2.zero, Quaternion.identity, group.transform);
            RectTransform rectTransform = displaySlot.gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = group.cellSize;
            displaySlot.Setup(inventoryReference, slot);
        }
    }
}
