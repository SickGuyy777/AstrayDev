using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RangeInteractor interactor;
    private Movement movement;
    
    private Vector2 mousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);


    private void Awake()
    {
        interactor = GetComponent<RangeInteractor>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if(PlayerInput.InteractKeyDown)
            interactor.Interact(this);
        
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector2 lookDirection = (mousePos - (Vector2) transform.position).normalized;
        
        movement.Move(PlayerInput.MovementDirection, Time.deltaTime);
        movement.LookInDirection(lookDirection);
    }

}