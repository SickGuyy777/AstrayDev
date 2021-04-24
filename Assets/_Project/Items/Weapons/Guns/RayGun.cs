using UnityEngine;
using Random = UnityEngine.Random;

public class RayGun : Weapon
{
    [Header("RayGun")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float inAccuracy = 5f;
    [SerializeField] private LayerMask mask;
    
    
    [Header("LineEffect")] 
    [SerializeField] private LineEffect lineEffectPrefab;
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private float lineWidth = 0.02f;
    [SerializeField] private float lineLifeTime = .5f;
    
    
    public override void Primary(IWeaponArgsHolder holder)
    {
        if(!canUse)
            return;

        StartCoroutine(WaitCD());

        WeaponArgs weaponArgs = holder.GetWeaponArgs();

        Vector2 offset = new Vector2(Random.Range(-inAccuracy, inAccuracy), Random.Range(-inAccuracy, inAccuracy)) / 150;
        Vector2 newDirection = (Vector2)weaponArgs.ray.direction + offset;
        
        WeaponArgs shootArgs = new WeaponArgs(new Ray(weaponArgs.ray.origin, newDirection), mask, weaponArgs.objectsToIgnore);
        Shoot(shootArgs);
    }

    private void Shoot(WeaponArgs shootArgs)
    {
        LineEffect createdLineEffect = Instantiate(lineEffectPrefab, transform.position, Quaternion.identity);
        createdLineEffect.Setup(firePoint.position, shootArgs.hit.point, lineColor, lineWidth, lineLifeTime);
        shootArgs.HitDamageable?.Damage(damage);
    }
}
