using UnityEngine;

public class PlayerController : MonoBehaviour, IWeaponArgsHolder
{
    [Header("Inventory")]
    [SerializeField] private GameObject backPackUI;
    [SerializeField] private Inventory backPack;

    private RangeInteractor interactor;
    private Movement movement;
    private WeaponHolder weaponHolder;
    private CharacterAnimator charAnimator;
    public static bool slow=false;
    
    public Inventory BackPack => backPack;
    private bool inventoryShown => backPackUI.activeSelf;


    private void Awake()
    {
        interactor = GetComponent<RangeInteractor>();
        movement = GetComponent<Movement>();
        weaponHolder = GetComponent<WeaponHolder>();
        charAnimator = GetComponent<CharacterAnimator>();
        
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
        bool moving = movement.Velocity.magnitude > .2f;

        movement.LookInDirection(lookDirection, Time.deltaTime);
        movement.Move(PlayerInput.MovementDirection, Time.deltaTime);
        charAnimator.SetWalkAnimation(moving);
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
    
    public WeaponArgs GetWeaponArgs() => new WeaponArgs(new Ray(transform.position, transform.right), new LayerMask(), this.gameObject);
    
    public Inventory GetAmmoSupply() => BackPack;
    public static void motion()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (slow == false)
            {
                slow = true;
            }
            else slow = false;
        }
    }
}