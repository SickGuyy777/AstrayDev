using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private Inventory inventoryReference;
    [SerializeField] private Transform group;
    [SerializeField] private ItemDisplaySlot displaySlotPrefab;


    private void Awake()
    {
        for (int i = 0; i < inventoryReference.Items.Length; i++)
        {
            ItemDisplaySlot displaySlot = Instantiate(displaySlotPrefab, Vector2.zero, Quaternion.identity, group.transform);
            displaySlot.Setup(inventoryReference, i);
            displaySlot.SetItemReference(inventoryReference.Items[i]);
        }
    }
}
