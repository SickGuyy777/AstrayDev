using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public string itemName = "New Item";
    public ItemTypeContainer.SetItemType itemType = ItemTypeContainer.SetItemType.None;
    public int maxStack = 16;
    public Sprite itemIcon;
    public ItemObject itemPrefab;
}
