using UnityEngine;

public class RangeInteractor : MonoBehaviour
{
    [SerializeField] private float range = 1;
    
    private IInteractable selectedInteractable;
    private GameObject selectedInteractableObject;


    private void Update() => UpdatedSelectedInteractable();

    public void Interact(PlayerController player)
    {
        selectedInteractable?.Interact(player);
    }
    
    private void UpdatedSelectedInteractable()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        IInteractable interactable = GetClosestInteractable(colliders, out GameObject selectedObj);

        if (selectedInteractable != interactable)
        {
            if (selectedInteractableObject == null)
                selectedInteractable = null;
            
            selectedInteractable?.Deselect();
            selectedInteractable = interactable;
            selectedInteractableObject = selectedObj;
            interactable?.Select();
        }
    }

    private IInteractable GetClosestInteractable(Collider2D[] colliders, out GameObject obj)
    {
        float closestRange = float.MaxValue;
        IInteractable closestInteractable = null;
        GameObject closestObj = null;

        foreach (Collider2D col in colliders)
        {
            float distance = Vector2.Distance(transform.position, col.transform.position);

            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null && distance < closestRange)
            {
                closestRange = range;
                closestInteractable = interactable;
                closestObj = col.gameObject;
            }
        }

        obj = closestObj;
        return closestInteractable;
    }
}
