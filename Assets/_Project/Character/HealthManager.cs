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
    public float Health
    {
        get => health;
        
        set
        {
            value = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChanged?.Invoke();

            if (value <= 0)
            {
                OnDied?.Invoke();
                return;
            }

            health = value;
        }

    }

    public event System.Action OnHealthChanged;
    public event System.Action OnDied;

    
    private void Awake()
    {
        foreach (var damageable in damageables)
        {
            damageable.OnDamaged += Damage;
        }
    }

    private void Damage(float damage) => Health -= damage;
}
