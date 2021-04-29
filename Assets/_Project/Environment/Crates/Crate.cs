using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private LootTable lootTable;

    private HealthManager healthManager;


    private void Awake()
    {
        healthManager = GetComponent<HealthManager>();
        
        healthManager.OnDied += Die;
    }

    private void Die()
    {
        Item[] loot = DropLoot();
        Destroy(this.gameObject);
    }

    private Item[] DropLoot()
    {
        Item[] loot = lootTable.GetLoot();

        foreach (Item item in loot)
        {
            item.Drop(transform.position);
        }

        return loot;
    }
}
