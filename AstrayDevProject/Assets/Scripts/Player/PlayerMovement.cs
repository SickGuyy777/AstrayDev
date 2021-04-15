using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Vector2 movement;
    public Vector2 mousePos;
    public Vector2 dir;
    public float Zangle;
    public float MovementSpeed = 10;

    //Components
    public Rigidbody2D Player;
    public Animator animator;

    public void Move()
    {

        if(movement.x == 0 && movement.y == 0) {

            animator.SetBool("Moving", false);

        } else if(movement.x != 0 || movement.y != 0) {

            animator.SetBool("Moving", true);

        }

        Player.MovePosition(Player.position + movement * MovementSpeed * Time.fixedDeltaTime);
        Player.rotation = Zangle;

    }

}
