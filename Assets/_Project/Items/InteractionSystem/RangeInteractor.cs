using UnityEngine;

public class RangeInteractor : MonoBehaviour
{
    [SerializeField] private float range = 1;

    private IInteractable selectedInteractable;
    private GameObject selectedInteractableObject;


    private void OnEnable() => TickClock.OnTick += UpdatedSelectedInteractable;
    
    private void OnDisable() => TickClock.OnTick -= UpdatedSelectedInteractable;

    public void Interact(PlayerController player)
    {
        selectedInteractable?.Interact(player);
    }

    private void UpdatedSelectedInteractable()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        if (colliders.Length == 0)
            return;

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
        float lowestMagnitude = int.MaxValue;
        IInteractable closestInteractable = null;
        GameObject closestObj = null;

        foreach (Collider2D col in colliders)
        {
            float magnitude = (col.transform.position - transform.position).magnitude;
            IInteractable interactable = col.gameObject.GetComponent<IInteractable>();
            if (interactable != null && magnitude < lowestMagnitude)
            {
                lowestMagnitude = magnitude;
                closestInteractable = interactable;
                closestObj = col.gameObject;
            }
        }

        obj = closestObj;
        return closestInteractable;
    }
}
