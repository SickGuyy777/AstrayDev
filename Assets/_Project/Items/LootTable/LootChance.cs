
using UnityEngine;

[System.Serializable]
public struct LootChance
{
    public SerializableItem serializableItem;
    [Range(0, 100)] public float chance;
    [Range(0, 99)] public int variety;


    public Item GetItem() => GetSerializeableItem().GetItem();
    public SerializableItem GetSerializeableItem() => new SerializableItem(serializableItem.info, serializableItem.amount + Random.Range(-variety, variety));
}
