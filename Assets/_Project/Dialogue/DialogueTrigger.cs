using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;
    private ISelectionResponse selectionResponse;



    public virtual void Awake(){
        selectionResponse = GetComponent<ISelectionResponse>();
    }


    public void TriggerDialogue(){
        FindObjectOfType<DialogueManager>().startDialogue(dialogue);
    }

    public void Select(){
        selectionResponse?.OnSelect();
    }
    public void Deselect(){
        selectionResponse?.OnDeselect();
    }
    public void Interact(PlayerController player){
        Debug.Log("test direct");
        TriggerDialogue();
    }
    
    

    
}

