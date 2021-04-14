using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PlayerMovement playerMovement;

    private void Update()
    {

        if(Input.GetKey(KeyCode.LeftShift)) {

            playerMovement.MovementSpeed = 10;

        } else {

            playerMovement.MovementSpeed = 5;

        }

        playerMovement.movement.x = Input.GetAxisRaw("Horizontal");
        playerMovement.movement.y = Input.GetAxisRaw("Vertical");

        playerMovement.mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerMovement.dir = playerMovement.mousePos - playerMovement.Player.position;
        playerMovement.Zangle = Mathf.Atan2(playerMovement.dir.y, playerMovement.dir.x) * Mathf.Rad2Deg;

    }

    private void FixedUpdate()
    {

        playerMovement.Move();

    }

}