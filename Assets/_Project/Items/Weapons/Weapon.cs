
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float useRate = .5f;
    protected Item item;
    protected bool canUse = true;

    private Coroutine waitCoroutine;
    
    public abstract void Primary(IWeaponArgsHolder holder);

    public void WaitCD(float duration = -1)
    {
        if(waitCoroutine != null)
            StopCoroutine(waitCoroutine);

        waitCoroutine = StartCoroutine(StartWaitCD(duration));
    }

    private IEnumerator StartWaitCD(float duration = -1)
    {
        duration = duration > 0 ? duration : useRate;

        canUse = false;
        yield return new WaitForSeconds(duration);
        canUse = true;

        waitCoroutine = null;
    }

    public void SetUp(Item itemReference) => this.item = itemReference;
}
