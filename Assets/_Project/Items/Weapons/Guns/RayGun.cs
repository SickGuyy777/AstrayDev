
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RayGun : Weapon
{
    [Header("RayGun")] 
    [SerializeField] private AmmoType ammoType = AmmoType.Rifle;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float inAccuracy = 5f;
    [SerializeField] private LayerMask mask;
    
    [Header("LineEffect")] 
    [SerializeField] private LineEffect lineEffectPrefab;
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private float lineWidth = 0.02f;
    [SerializeField] private float lineLifeTime = .5f;

    private Queue<AmmoComponent> ammo = new Queue<AmmoComponent>();
    private Inventory bulletSupply;

    
    public override void Primary(IWeaponArgsHolder holder)
    {
        if(!canUse)
            return;

        if (bulletSupply == null)
        {
            bulletSupply = holder.GetAmmoSupply();
            bulletSupply.OnChanged += UpdateAmmo;
        }

        UpdateAmmo();
        
        StartCoroutine(WaitCD());
        
        if (ammo.Count <= 0)
        {
            Debug.Log("out of ammo");
            return;
        }
        
        ammo.Peek().Item.Amount--;
        
        if (ammo.Peek().Item.Amount <= 0)
            ammo.Dequeue();
        
        
        WeaponArgs weaponArgs = holder.GetWeaponArgs();

        Vector2 offset = new Vector2(Random.Range(-inAccuracy, inAccuracy), Random.Range(-inAccuracy, inAccuracy)) / 150;
        Vector2 newDirection = (Vector2)weaponArgs.ray.direction + offset;
        
        WeaponArgs shootArgs = new WeaponArgs(new Ray(weaponArgs.ray.origin, newDirection), mask, weaponArgs.objectsToIgnore);
        Shoot(shootArgs);
    }

    private void UpdateAmmo() => ammo = GetAmmo();
    
    private Queue<AmmoComponent> GetAmmo()
    {
        Queue<AmmoComponent> sameTypeBullets = new Queue<AmmoComponent>();
        AmmoComponent[] bullets = bulletSupply.GetItemsOfType<AmmoComponent>();
        
        foreach (AmmoComponent bullet in bullets)
        {
            if(bullet.IsSameType(ammoType))
                sameTypeBullets.Enqueue(bullet);
        }
        
        return sameTypeBullets;
    }

    private void Shoot(WeaponArgs shootArgs)
    {
        LineEffect createdLineEffect = Instantiate(lineEffectPrefab, transform.position, Quaternion.identity);
        createdLineEffect.Setup(firePoint.position, shootArgs.hit.point, lineColor, lineWidth, lineLifeTime);
        shootArgs.HitDamageable?.Damage(damage);
    }
}
