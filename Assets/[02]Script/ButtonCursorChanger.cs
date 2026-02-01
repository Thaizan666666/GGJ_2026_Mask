using UnityEngine;

public class ButtonCursorChanger : MonoBehaviour
{
    [Header("Cursor Sprites")]
    public Texture2D hoverCursor;
    public Texture2D clickCursor;
    public Texture2D normalCursor;

    [Header("Hotspot (จุดคลิก)")]
    public Vector2 hotspot = new Vector2(16, 16); // กลางรูป 32x32

    // ฟังก์ชันสำหรับเรียกจาก EventTrigger
    public void OnHover()
    {
        if (hoverCursor != null)
            CursorManager.Instance.ChangeCursor(hoverCursor, hotspot);
    }

    public void m_OnClick()
    {
        if (clickCursor != null)
            CursorManager.Instance.ChangeCursor(clickCursor, hotspot);
    }

    public void OnExit()
    {
        if (normalCursor != null)
            CursorManager.Instance.ChangeCursor(normalCursor, hotspot);
        else
            CursorManager.Instance.ResetCursor();
    }
}