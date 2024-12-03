using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor; // Assign in Inspector
    public Texture2D combatCursor; // Assign in Inspector

    void Start()
    {
        // Set default cursor at start
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    // Example method to change cursor
    public void SetCombatCursor()
    {
        Cursor.SetCursor(combatCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    // Reset cursor to default
    public void ResetCursor()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}