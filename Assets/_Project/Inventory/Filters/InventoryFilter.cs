using UnityEngine;

public abstract class InventoryFilter : MonoBehaviour
{
    protected abstract ItemComponent[] GetFunctionalityWhitelist();
    
    public bool ContainedInWhitelist(Item item)
    {
        ItemComponent[] filterFunctionalities = GetFunctionalityWhitelist();
        
        foreach (ItemComponent functionality in item.Components)
        {
            foreach (ItemComponent filterFunctionality in filterFunctionalities)
            {
                if (functionality.GetType() == filterFunctionality.GetType())
                    return true;
            }
        }

        return false;
    }
}
