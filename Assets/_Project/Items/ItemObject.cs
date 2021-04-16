using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemInfo info;
    
    private ISelectionResponse selectionResponse;
    
    public Item Item { get; private set; }
    
    
    protected virtual void Awake()
    {
        Item = new Item(info, 1);
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
        player.Inventory.AddToInventory(this.Item);
        Destroy(this.gameObject);
    }
}
