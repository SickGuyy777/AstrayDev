using UnityEngine;

public static class PlayerInput
{
    public static Vector2 MousePos => Camera.main != null ? (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) : Vector2.zero;
    public static Vector2 MovementDirection => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    public static bool InteractKeyDown => Input.GetKeyDown(KeyCode.E);
    public static bool InventoryKeyDown => Input.GetKeyDown(KeyCode.I);
}
