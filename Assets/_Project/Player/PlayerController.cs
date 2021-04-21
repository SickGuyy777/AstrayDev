using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private GameObject backPackUI;
    [SerializeField] private Inventory backPack;
    
    private RangeInteractor interactor;
    private Movement movement;
    private WeaponHolder weaponHolder;
    
    public Inventory BackPack => backPack;
    private bool inventoryShown => backPackUI.activeSelf;


    private void Awake()
    {
        interactor = GetComponent<RangeInteractor>();
        movement = GetComponent<Movement>();
        weaponHolder = GetComponent<WeaponHolder>();
        
        backPackUI.SetActive(false);
    }

    private void Update()
    {
        if(PlayerInput.InteractKeyDown)
            interactor.Interact(this);

        if (PlayerInput.InventoryKeyDown)
            ToggleInventory();
        
        if(PlayerInput.IsScrolling)
            weaponHolder.ScrollEquip((int)Mathf.Clamp(PlayerInput.ScrollDelta * float.MaxValue, -1, 1));
        
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector2 lookDirection = (PlayerInput.MousePos - (Vector2) transform.position).normalized;
        
        movement.Move(PlayerInput.MovementDirection, Time.deltaTime);
        movement.LookInDirection(lookDirection);
    }

    private void InventoryOn()
    {
        InventoryCursor.Instance?.Drop();
        backPackUI.SetActive(true);
    }

    private void InventoryOff()
    {
        InventoryCursor.Instance?.Drop();
        backPackUI.SetActive(false);
    }
    
    private void ToggleInventory()
    {
        if(inventoryShown)
            InventoryOff();
        else
            InventoryOn();
    }
}