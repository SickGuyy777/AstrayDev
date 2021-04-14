using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PlayerMovement playerMovement;
    Vector2 movement;
    Vector2 mousePos;
    Vector2 dir;
    float Zangle;
    float MovementSpeed = 10;

    private void Update()
    {

        if(Input.GetKey(KeyCode.LeftShift)) {

            MovementSpeed = 20;

        } else {

            MovementSpeed = 10;

        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir = mousePos - playerMovement.Player.position;
        Zangle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

    }

    private void FixedUpdate()
    {

        playerMovement.Move(movement, Zangle, MovementSpeed);

    }

}