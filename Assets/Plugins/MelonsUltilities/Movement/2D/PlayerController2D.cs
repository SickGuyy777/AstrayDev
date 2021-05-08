using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    private PlayerMovement playerMovement;
    
    
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement() => playerMovement.Move(GetMovementDirection(), Time.deltaTime);

    private Vector2 GetMovementDirection() => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
}