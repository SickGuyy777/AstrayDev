using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private float health = 10;
    
    [Header("Damageables")]
    [SerializeField] private List<DamageableObject> damageables = new List<DamageableObject>();
    
    public float MaxHealth => maxHealth;
    public float HealthPercentage => Mathf.InverseLerp(0, maxHealth, health);
    public float ReversedHealthPercentage => Mathf.InverseLerp(maxHealth, 0, health);
    public float Health
    {
        get => health;
        
        set
        {
            value = Mathf.Clamp(value, 0, MaxHealth);

            if (value <= 0)
            {
                health = value;
                OnHealthChanged?.Invoke();
                OnDied?.Invoke();
                return;
            }

            health = value;
            OnHealthChanged?.Invoke();
        }

    }

    public event Action OnHealthChanged;
    public event Action OnDied;

    
    private void Awake()
    {
        foreach (var damageable in damageables)
        {
            damageable.OnDamaged += Damage;
        }
    }
   
    private void Damage(float damage) => Health -= damage;
}
