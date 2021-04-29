using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LootTable
{
    public LootChance[] table;
    
    
    public Item[] GetLoot()
    {
        List<Item> itemsToAdd = new List<Item>();
        
        foreach (LootChance lootChance in table)
        {
            float randNum = Random.Range(0f, 100f);
            bool canAddLoot = lootChance.chance >= randNum;
            
            if(!canAddLoot)
                continue;
            
            itemsToAdd.Add(lootChance.GetItem());
        }

        return itemsToAdd.ToArray();
    }

    private LootChance GetRandomLootChance()
    {
        int randIndex = Random.Range(0, table.Length);
        return table[randIndex];
    }
}
