using UnityEngine;

public class DamageableObject : MonoBehaviour, IDamageable
{
    [SerializeField] private float damageMultiplier = 1;
    
    public bool IsCritical => damageMultiplier > 1;


    public void Damage(float amount)
    {
        float totalDamage = GetDamage(amount);
        Debug.Log("Took " + totalDamage + " damage!");
    }

    public float GetDamage(float damage) => damage * damageMultiplier;
}
