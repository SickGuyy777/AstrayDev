using UnityEngine;

public class GunFunctionality : Functionality
{
    public GameObject gunPrefab;


    public GunFunctionality(GameObject gunPrefab)
    {
        this.gunPrefab = gunPrefab;
    }
    
    public override void Execute()
    {
        
    }
    
    public GameObject SpawnGun(Transform handPos)
    {
        GameObject gunObj = GameObject.Instantiate(gunPrefab, handPos.position, Quaternion.identity);
        
        return gunObj;
    }
}
