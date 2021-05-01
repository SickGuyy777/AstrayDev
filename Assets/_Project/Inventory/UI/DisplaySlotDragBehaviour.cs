using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DisplaySlot))]
public class DisplaySlotDragBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static DisplaySlotDragBehaviour currentSelectedDragable;
    public DisplaySlot DisplaySlot { get; private set; }


    private void Awake() => DisplaySlot = GetComponent<DisplaySlot>();

    public void OnPointerEnter(PointerEventData eventData) => currentSelectedDragable = this;

    public void OnPointerExit(PointerEventData eventData) => currentSelectedDragable = null;
}
