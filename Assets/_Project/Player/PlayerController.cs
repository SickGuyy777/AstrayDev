using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private GameObject backPackDisplay;
    [SerializeField] private Inventory backPack;
    private bool inventoryShown => backPackDisplay.activeSelf;

    private RangeInteractor interactor;
    private Movement movement;
    public Inventory BackPack => backPack;


    private void Awake()
    {
        interactor = GetComponent<RangeInteractor>();
        movement = GetComponent<Movement>();
        
        backPackDisplay.SetActive(false);
    }

    private void Update()
    {
        if(PlayerInput.InteractKeyDown)
            interactor.Interact(this);

        if (PlayerInput.InventoryKeyDown)
            ToggleInventory();
        
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector2 lookDirection = (PlayerInput.MousePos - (Vector2) transform.position).normalized;
        
        movement.Move(PlayerInput.MovementDirection, Time.deltaTime);
        movement.LookInDirection(lookDirection);
    }

    private void ToggleInventory()
    {
        if(inventoryShown)
            backPackDisplay.SetActive(false);
        else
            backPackDisplay.SetActive(true);
    }
}