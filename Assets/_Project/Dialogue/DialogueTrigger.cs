using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;
    private ISelectionResponse selectionResponse;

    
    private void Awake()
    {
        selectionResponse = GetComponent<ISelectionResponse>();
    }

    public void Select()
    {
        selectionResponse?.OnSelect();
    }
    
    public void Deselect()
    {
        selectionResponse?.OnDeselect();
    }
    
    public void Interact(PlayerController player)
    {
        Debug.Log("test direct");
    }
}

