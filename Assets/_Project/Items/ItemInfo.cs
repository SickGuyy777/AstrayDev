using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public string itemName = "New Item";
    public int maxStack = 16;
    public Sprite itemIcon;
    public ItemObject itemPrefab;
}
