using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    public bool isInArea;

    public virtual void Update() {

        if(isInArea && Input.GetKeyDown(KeyCode.E)) {

            Interact();

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        isInArea = true;

    }

    private void OnTriggerExit2D(Collider2D other)
    {

        isInArea = false;

    }

    public abstract void Interact();

}