using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public bool isSprinting;
    public float MovementSpeed;
    public Transform Player;

    private void Update()
    {

        if(Input.GetKey(KeyCode.LeftShift))
        {

            isSprinting = true;
            MovementSpeed = 20;

        } else
        {

            isSprinting = false;
            MovementSpeed = 10;

        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 pos = new Vector3(x * MovementSpeed * Time.deltaTime, y * MovementSpeed * Time.deltaTime, 0);

        Player.transform.position += pos;

    }

}
