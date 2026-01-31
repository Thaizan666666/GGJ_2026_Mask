using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ChangeCursor(Texture2D cursorTexture, Vector2 hotspot)
    {
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}