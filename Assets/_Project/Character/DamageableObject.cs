using System;
using UnityEngine;

public class DamageableObject : MonoBehaviour, IDamageable
{
    [SerializeField] private float damageMultiplier = 1;
    
    public event Action<float, DamageableObject> OnDamaged = delegate { };
    public bool IsCritical => damageMultiplier > 1;


    public void Damage(float amount)
    {
        float totalDamage = GetDamage(amount);
        OnDamaged?.Invoke(totalDamage, this);
    }

    public float GetDamage(float damage) => damage * damageMultiplier;
}
