using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private Inventory inventoryReference;
    [SerializeField] private GridLayoutGroup group;
    [SerializeField] private InventoryDisplaySlot displaySlotPrefab;

    private InventoryDisplaySlot[] displaySlots;
    public bool isOn => display.activeSelf;


    private void Awake()
    {
        displaySlots = new InventoryDisplaySlot[inventoryReference.Items.Length];
        
        for (int i = 0; i < inventoryReference.Items.Length; i++)
        {
            InventoryDisplaySlot displaySlot = Instantiate(displaySlotPrefab, Vector2.zero, Quaternion.identity, group.transform);
            displaySlots[i] = displaySlot;
            
            displaySlot.SetItemReference(inventoryReference.Items[i]);
            displaySlot.Setup(inventoryReference, i);
        }
    }
    
    public void TurnOn() => display.SetActive(true);

    public void TurnOff() => display.SetActive(false);

    public void Toggle()
    {
        if(isOn)
            TurnOff();
        else
            TurnOn();
    }
}
