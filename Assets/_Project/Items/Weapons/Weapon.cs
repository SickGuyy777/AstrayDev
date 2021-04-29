using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float useRate = .5f;
    protected Item item;
    protected bool canUse = true;


    public abstract void Primary(IWeaponArgsHolder holder);

    protected void WaitCD()
    {
        canUse = false;
        Invoke(nameof(ResetCD), useRate);
    }

    private void ResetCD() => canUse = true;

    public void SetUp(Item itemReference) => this.item = itemReference;
}
