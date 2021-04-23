using UnityEngine;

public class RayGun : Weapon
{
    [Header("RayGun")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float damage;
    
    
    public override void Primary(IWeaponArgsHolder holder)
    {
        if(!canUse)
            return;

        StartCoroutine(WaitCD());
        Shoot(holder.GetWeaponArgs());
    }

    private void Shoot(WeaponArgs shootArgs)
    {
        Debug.Log("Shot!");
        Debug.DrawLine(shootArgs.ray.origin, shootArgs.hit.point, Color.red, .6f);
        shootArgs.HitDamageable?.Damage(damage);
    }
}
