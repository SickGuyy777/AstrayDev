using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private AreaTrigger damageTrigger;

    [SerializeField] private float damage;
    private Coroutine damageCoroutine;
    
    
    public override void Primary(IWeaponArgsHolder holder)
    {
        if(!canUse)
            return;
        
        DamageTrigger(.15f);
        WaitCD();
    }
    
    private void PlayAnimation(float duration)
    {
        Sequence sequence = DOTween.Sequence().SetAutoKill(true);
        sequence.Append(transform.DOLocalMove(Vector2.right, duration / 2, false));
        sequence.Append(transform.DOLocalMove(Vector3.zero, duration / 2, false));
    }

    private void DamageTrigger(float duration)
    {
        if (damageCoroutine != null)
        {
            transform.localPosition = Vector2.zero;
            StopCoroutine(damageCoroutine);
        }

        damageCoroutine = StartCoroutine(StartDamageTrigger(duration));
    }

    private IEnumerator StartDamageTrigger(float duration)
    {
        PlayAnimation(duration);
        List<IDamageable> hitDamageables = new List<IDamageable>();
        
        while (duration > 0)
        {
            IDamageable[] damageables = damageTrigger.GetObjects<IDamageable>();
            
            foreach (IDamageable damageable in damageables)
            {
                if(hitDamageables.Contains(damageable))
                    continue;
                
                damageable.Damage(damage);
                hitDamageables.Add(damageable);
            }

            duration -= Time.deltaTime;
            yield return null;
        }

        damageCoroutine = null;
    }
}
