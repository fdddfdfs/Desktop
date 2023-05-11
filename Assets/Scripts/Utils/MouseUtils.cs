using UnityEngine;
using UnityEngine.InputSystem;

public static class MouseUtils
{
    public static Vector2 GetMousePosition()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        mousePosition -= new Vector2(Screen.width, Screen.height) / 2;

        return mousePosition;
    }
}