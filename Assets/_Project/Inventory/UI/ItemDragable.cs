using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemDisplaySlot))]
public class ItemDragable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static ItemDragable currentSelectedDragable;
    public ItemDisplaySlot DisplaySlot { get; private set; }


    private void Awake() => DisplaySlot = GetComponent<ItemDisplaySlot>();

    public void OnPointerEnter(PointerEventData eventData) => currentSelectedDragable = this;

    public void OnPointerExit(PointerEventData eventData) => currentSelectedDragable = null;
}
