using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventoryDisplaySlot))]
public class ItemDragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private static ItemDragable currentSelectedDragable;
    private InventoryDisplaySlot displaySlot;
    private bool pendingDragAction = false;


    private void Awake()
    {
        displaySlot = GetComponent<InventoryDisplaySlot>();
    }

    private void Update()
    {
        if (pendingDragAction)
        {
            if (currentSelectedDragable != null)
            {
                Item selectedItemSlot = currentSelectedDragable.displaySlot.ItemSlot;
                Item draggingItem = PlayerCursor.Instance.HoldingItem;

                if (selectedItemSlot.IsEmpty)
                {
                    PlayerCursor.Instance.PlaceItem(currentSelectedDragable.displaySlot);
                    pendingDragAction = false;
                }
                else
                {
                    PlayerCursor.Instance.SwapItem(currentSelectedDragable.displaySlot);
                    pendingDragAction = false;
                }
                    
            }
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                PlayerCursor.Instance.HoldingItem.Drop(displaySlot.Inventory.transform.position);
                pendingDragAction = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(pendingDragAction)
            return;
        
        PlayerCursor.Instance.GrabItem(displaySlot);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(pendingDragAction)
            return;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(pendingDragAction)
            return;
        
        pendingDragAction = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentSelectedDragable = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(currentSelectedDragable == this)
            currentSelectedDragable = null;
    }
}
