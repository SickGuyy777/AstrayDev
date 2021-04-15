using UnityEngine;

public class TestItem : MonoBehaviour, IInteractable
{
    private ISelectionResponse selectionResponse;


    private void Awake()
    {
        selectionResponse = GetComponent<ISelectionResponse>();
    }

    public void Select()
    {
        Debug.Log("Selected: " + gameObject);
        selectionResponse?.OnSelect();
    }

    public void Deselect()
    {
        Debug.Log("Deselected: " + gameObject);
        selectionResponse?.OnDeselect();
    }

    public void Interact(PlayerController player)
    {
        Debug.Log("Interacted with: " + gameObject);
        Destroy(this.gameObject);
    }
}
