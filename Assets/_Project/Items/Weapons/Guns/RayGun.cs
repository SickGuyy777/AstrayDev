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
    [SerializeField] private float numOfProjectiles = 1;
    [SerializeField] private LayerMask mask;
    
    [Header("LineEffect")] 
    [SerializeField] private LineEffect lineEffectPrefab;
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private float lineWidth = 0.02f;
    [SerializeField] private float lineLifeTime = .5f;
    private ObjectPool<LineEffect> lineEffectPool;

    private Queue<AmmoComponent> ammoItems = new Queue<AmmoComponent>();
    private Inventory bulletSupply;


    private void Awake()
    {
        int poolSize = Mathf.CeilToInt(Mathf.CeilToInt(Mathf.Clamp(numOfProjectiles * (lineLifeTime / useRate), numOfProjectiles, int.MaxValue)));
        lineEffectPool = new ObjectPool<LineEffect>(lineEffectPrefab, poolSize);
    }
    
    public override void Primary(IWeaponArgsHolder holder)
    {
        if(!canUse || !gameObject.activeInHierarchy)
            return;

        Shoot(holder);
    }
    
    private void UpdateAmmo()
    {
        if(!gameObject.activeInHierarchy)
            return;
        
        Queue<AmmoComponent> sameTypeBullets = new Queue<AmmoComponent>();
        List<AmmoComponent> ammoInInventory = bulletSupply.GetItemsOfType<AmmoComponent>();
        
        foreach (AmmoComponent bullet in ammoInInventory)
        {
            if(bullet.IsSameType(ammoType) && !bullet.Item.IsEmpty)
                sameTypeBullets.Enqueue(bullet);
        }
        
        ammoItems = sameTypeBullets;
    }

    private void Shoot(IWeaponArgsHolder holder)
    {
        if(!gameObject.activeInHierarchy)
            return;
        
        if (bulletSupply == null)
            bulletSupply = holder.GetAmmoSupply();

        UpdateAmmo();
        WaitCD();
        
        if (ammoItems.Count <= 0)
            return;
        
        WeaponArgs weaponArgs = holder.GetWeaponArgs();
        ammoItems.Peek().Item.Amount--;

        for (int i = 0; i < numOfProjectiles; i++)
        {
            float x = Random.Range(-inAccuracy, inAccuracy) / 150;
            float y = Random.Range(-inAccuracy, inAccuracy) / 150;
            
            Vector2 offset = new Vector2(x, y);
            Vector2 newDirection = (Vector2)weaponArgs.ray.direction + offset;
            
            WeaponArgs shootArgs = new WeaponArgs(new Ray(weaponArgs.ray.origin, newDirection), mask, weaponArgs.objectsToIgnore);
            LineEffect createdLineEffect = lineEffectPool.Instantiate(transform.position, Quaternion.identity);
            createdLineEffect.Setup(firePoint.position, shootArgs.hit.point, lineColor, lineWidth, lineLifeTime);
        
            shootArgs.HitDamageable?.Damage(damage);
        }
    }

    private void OnDestroy()
    {
        if(bulletSupply != null) 
            bulletSupply.OnChanged -= UpdateAmmo;
        
        lineEffectPool.DestroyPool();
    }
}
