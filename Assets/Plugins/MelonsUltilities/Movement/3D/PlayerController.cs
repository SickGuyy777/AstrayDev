using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;


    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        HandleInput();
        UpdateMovement();
        UpdateLook();
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            movement.Jump();
    }

    private void UpdateMovement() => movement.Move(GetMovementDirection(), Time.deltaTime);

    private void UpdateLook() => movement.UpdateLook();
    
    private Vector3 GetMovementDirection() => new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    
    private Vector2 GetLookDelta() => new Vector3(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
}
