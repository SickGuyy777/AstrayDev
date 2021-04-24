using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Setup")]

    [SerializeField]
    private List<DamageableObject> damageables = new List<DamageableObject>();

    [SerializeField]
    private bool getFromChildren;

    [SerializeField]
    private float health = 100f;

    [SerializeField]
    private float maxHealth = 120f;

    public float Health
    {
        get => health;
        set
        {
            if (value == health)
                return;
            value = Mathf.Clamp(value, 0, maxHealth);
            HealthChanged?.Invoke();

            if (value <= 0)
            {
                Die?.Invoke();
                return;
            }

            health = value;
        }

    }

    public event System.Action HealthChanged;
    public event System.Action Die;

    private void Start()
    {
        Health = Mathf.Clamp(Health, 0, maxHealth);

        if (getFromChildren)
        {
            DamageableObject[] components = GetComponentsInChildren<DamageableObject>();
            for (int i = 0; i < components.Length; i++)
                damageables.Add(components[i]);
        }

        foreach (var damageable in damageables)
            damageable.OnDamaged += Damaged;
    }

    private void Damaged(float damage, DamageableObject obj) => health -= damage;
}
