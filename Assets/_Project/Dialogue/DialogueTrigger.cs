using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;
    private ISelectionResponse selectionResponse;

    
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

