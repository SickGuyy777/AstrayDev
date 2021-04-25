using System.Collections.Generic;
using UnityEngine;

public class MutantSpawner : MonoBehaviour
{
    [SerializeField] private int maxAmount = 5;
    [SerializeField] private BasicMutantAI mutantPrefab;
    [SerializeField] private float spawnRate = 1;

    private List<BasicMutantAI> createdMutants = new List<BasicMutantAI>();
    private ObjectPool<BasicMutantAI> spawnPool;

    
    private void Start()
    {
        spawnPool = new ObjectPool<BasicMutantAI>(mutantPrefab, maxAmount);
        
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
        if(createdMutants.Count >= maxAmount)
            return;
        
        BasicMutantAI createdMutant = spawnPool.Instantiate(transform.position, Quaternion.identity);
        createdMutants.Add(createdMutant);
        
        createdMutant.HealthManager.OnDied += () => createdMutants.Remove(createdMutant);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector2.one / 2f);
    }
}
