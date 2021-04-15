using UnityEngine;

public static class PlayerInput
{
    public static Vector2 MovementDirection => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    public static bool InteractKeyDown => Input.GetKeyDown(KeyCode.E);
}
