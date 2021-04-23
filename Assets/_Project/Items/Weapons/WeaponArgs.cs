using System.Linq;
using UnityEngine;

public struct WeaponArgs
{
    public Ray ray;
    public RaycastHit2D hit;
    public Collider2D hitCollider;
    public readonly GameObject[] objectsToIgnore;


    public IDamageable HitDamageable => hitCollider?.GetComponent<IDamageable>();

    public PlayerCharacter HitPlayer => hitCollider?.GetComponentInParent<PlayerCharacter>();
    

    public WeaponArgs(Ray ray, params GameObject[] objectsToIgnore)
    {
        this.ray = ray;
        this.objectsToIgnore = objectsToIgnore;
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        
        hit = new RaycastHit2D();
        hit.point = ray.direction * 9999f;
        hitCollider = null;

        foreach (RaycastHit2D rayHit in SortRayCastAllHits(ray.origin, hits))
        {
            if (rayHit.collider == null)
                continue;

            if (!IsIgnoredObject(rayHit.collider.gameObject, objectsToIgnore))
            {
                hit = rayHit;
                hitCollider = hit.collider;
                break;
            }
        }
    }

    private bool IsIgnoredObject(GameObject checkObject, GameObject[] newObjectsToIgnore)
    {
        Transform root = checkObject.transform.root;
        
        if (newObjectsToIgnore == null)
            return false;

        if (newObjectsToIgnore.Length <= 0)
            return false;
        
        if(root.gameObject == checkObject)
            return newObjectsToIgnore.Contains(checkObject);
        
        for (int i = 0; i < root.childCount; i++)
        {
            GameObject childedObject = root.GetChild(i).gameObject;

            if (newObjectsToIgnore.Contains(childedObject))
                return true;
            
            if(root?.GetChild(i).gameObject == checkObject)
                break;
        }

        return false;
    }

    private RaycastHit2D[] SortRayCastAllHits(Vector2 shootPoint, RaycastHit2D[] allHits)
    {
        return allHits.OrderBy(newHit => Vector2.Distance(shootPoint, newHit.point)).ToArray();
    }
}
