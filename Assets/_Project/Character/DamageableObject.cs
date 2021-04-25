using System;
using UnityEngine;

public class DamageableObject : MonoBehaviour, IDamageable
{
    [SerializeField] private float damageMultiplier = 1;
    
    public event Action<float> OnDamaged = delegate { };
    public bool IsCritical => damageMultiplier > 1;


    public void Damage(float amount)
    {
        float totalDamage = GetDamage(amount);
        OnDamaged?.Invoke(totalDamage);
    }

    public float GetDamage(float damage) => damage * damageMultiplier;
}
