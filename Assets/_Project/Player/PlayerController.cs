using UnityEngine;

public class PlayerController : MonoBehaviour, IWeaponArgsHolder
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
        HandleInput();
        
        UpdateMovement();
    }

    private void HandleInput()
    {
        if (PlayerInput.InventoryKeyDown)
            ToggleInventory();
        
        if (PlayerInput.InteractKeyDown)
            interactor.Interact(this);

        if (PlayerInput.IsScrolling)
            weaponHolder.ScrollEquip((int) Mathf.Clamp(PlayerInput.ScrollDelta * float.MaxValue, -1, 1));
        
        if (PlayerInput.PrimaryFire && !inventoryShown)
            weaponHolder.HoldingWeapon?.Primary(this);
    }

    private void UpdateMovement()
    {
        Vector2 lookDirection = !inventoryShown ? (PlayerInput.MousePos - (Vector2) transform.position).normalized : (Vector2)transform.right;

        movement.LookInDirection(lookDirection);
        movement.Move(PlayerInput.MovementDirection, Time.deltaTime);
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
    
    public WeaponArgs GetWeaponArgs() => new WeaponArgs(new Ray(transform.position, transform.right), this.gameObject);
}