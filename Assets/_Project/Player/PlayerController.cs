using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InventoryDisplay inventoryDisplay;
    
    private Inventory inventory;
    private RangeInteractor interactor;
    private Movement movement;

    public Inventory Inventory => inventory;


    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        interactor = GetComponent<RangeInteractor>();
        movement = GetComponent<Movement>();
        
        inventoryDisplay.TurnOff();
    }

    private void Update()
    {
        if(PlayerInput.InteractKeyDown)
            interactor.Interact(this);
        
        if(PlayerInput.InventoryKeyDown)
            inventoryDisplay.Toggle();
        
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector2 lookDirection = (PlayerInput.MousePos - (Vector2) transform.position).normalized;
        
        movement.Move(PlayerInput.MovementDirection, Time.deltaTime);
        movement.LookInDirection(lookDirection);
    }

}