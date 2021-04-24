using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private float useTime = .5f;
    protected Item item;
    protected bool canUse = true;


    public abstract void Primary(IWeaponArgsHolder holder);

    protected IEnumerator WaitCD()
    {
        canUse = false;
        yield return new WaitForSeconds(useTime);
        canUse = true;
    }

    public void SetItemReference(Item itemReference) => this.item = itemReference;
}
