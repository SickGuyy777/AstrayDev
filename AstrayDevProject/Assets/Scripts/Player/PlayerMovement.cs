using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Components
    public Rigidbody2D Player;
    public Animator animator;

    public void Move(Vector2 movement, float Zangle, float MovementSpeed)
    {

        Player.MovePosition(Player.position + movement * MovementSpeed * Time.fixedDeltaTime);
        Player.rotation = Zangle;

    }

}
