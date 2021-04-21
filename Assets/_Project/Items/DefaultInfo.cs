using UnityEngine;

[CreateAssetMenu(menuName = "Items/DefaultInfo")]
public class DefaultInfo : ItemInfo
{
    protected override Functionality[] GetFunctionalities()
    {
        return new Functionality[]
        {
            new ItemFunctionality()
        };
    }
}
