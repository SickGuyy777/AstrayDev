using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemInfo info;
    
    private ISelectionResponse selectionResponse;
    
    public Item Item { get; private set; }
    
    
    protected virtual void Awake()
    {
        Item = info.GetNewItem();
        selectionResponse = GetComponent<ISelectionResponse>();
    }

    public void Select()
    {
        selectionResponse?.OnSelect();
    }

    public void Deselect()
    {
        selectionResponse?.OnDeselect();
    }

    public void Interact(PlayerController player)
    {
        player.BackPack.AddToInventory(this.Item.Transfer());
        Destroy(this.gameObject);
    }
}
