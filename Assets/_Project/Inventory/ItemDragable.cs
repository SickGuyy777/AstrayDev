using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventoryDisplaySlot))]
public class ItemDragable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static ItemDragable currentSelectedDragable;
    public InventoryDisplaySlot DisplaySlot { get; private set; }


    private void Awake() => DisplaySlot = GetComponent<InventoryDisplaySlot>();

    public void OnPointerEnter(PointerEventData eventData) => currentSelectedDragable = this;

    public void OnPointerExit(PointerEventData eventData) => currentSelectedDragable = null;
}
