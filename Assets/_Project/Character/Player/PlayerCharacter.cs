using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IWeaponArgsHolder
{
    [Header("Inventory")]
    [SerializeField] private GameObject backPackUI;
    [SerializeField] private Inventory backPack;

    private RangeInteractor interactor;
    private CharacterMovement characterMovement;
    private WeaponHolder weaponHolder;
    private CharacterAnimator charAnimator;
    
    public Inventory BackPack => backPack;
    private bool inventoryShown => backPackUI.activeSelf;


    private void Awake()
    {
        interactor = GetComponent<RangeInteractor>();
        characterMovement = GetComponent<CharacterMovement>();
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
        bool moving = characterMovement.Velocity.magnitude > .2f;

        characterMovement.LookInDirection(lookDirection);
        characterMovement.Move(PlayerInput.MovementDirection, Time.deltaTime);
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
}