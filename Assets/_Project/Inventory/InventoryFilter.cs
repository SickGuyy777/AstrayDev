using UnityEngine;

public abstract class InventoryFilter : MonoBehaviour
{
    protected abstract Functionality[] GetFunctionalityWhitelist();
    
    public bool ContainedInWhitelist(Item item)
    {
        Functionality[] filterFunctionalities = GetFunctionalityWhitelist();
        
        foreach (Functionality functionality in item.Functionalities)
        {
            foreach (Functionality filterFunctionality in filterFunctionalities)
            {
                if (functionality.GetType() == filterFunctionality.GetType())
                    return true;
            }
        }

        return false;
    }
}
