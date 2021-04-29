using System.Collections;
using UnityEngine;

public class RangeInteractor : MonoBehaviour
{
    [SerializeField] private float range = 1;
    [SerializeField] private float searchCooldown = 0.02f;
    
    private IInteractable selectedInteractable;
    private GameObject selectedInteractableObject;

    
    private void Awake()
    {
        StartCoroutine(SearchForInteractables());
    }

    public void Interact(PlayerController player)
    {
        selectedInteractable?.Interact(player);
    }
    
    private IEnumerator SearchForInteractables()
    {
        while (true)
        {
            yield return new WaitForSeconds(searchCooldown);
            UpdatedSelectedInteractable();
        }
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
            IInteractable interactable = col.GetComponent<IInteractable>();
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
