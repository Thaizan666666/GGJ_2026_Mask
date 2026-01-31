using UnityEngine;
using UnityEngine.EventSystems;

public class CustomCursorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Cursor Sprites")]
    public Texture2D normalCursor;      // Cursor ปกติ
    public Texture2D hoverCursor;       // Cursor เมื่อ Hover
    public Texture2D clickCursor;       // Cursor เมื่อคลิก

    public bool isClick;

    [Header("Cursor Settings")]
    public Vector2 hotspot = Vector2.zero; // จุดที่ใช้คลิก (มุมบนซ้าย = 0,0)
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        // ตั้ง Cursor เริ่มต้น
        if (normalCursor != null)
        {
            Cursor.SetCursor(normalCursor, hotspot, cursorMode);
        }
        isClick = false;
    }

    // เมื่อเมาส์เข้ามาใน Button
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverCursor != null && !isClick)
        {
            Cursor.SetCursor(hoverCursor, hotspot, cursorMode);
            Debug.Log("Hover: เปลี่ยน Cursor");
        }
    }

    // เมื่อเมาส์ออกจาก Button
    public void OnPointerExit(PointerEventData eventData)
    {
        if (normalCursor != null && !isClick)
        {
            Cursor.SetCursor(normalCursor, hotspot, cursorMode);
            Debug.Log("Exit: กลับเป็น Cursor ปกติ");
        }
    }

    // เมื่อกดปุ่มเมาส์ลง
    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickCursor != null)
        {
            Cursor.SetCursor(clickCursor, hotspot, cursorMode);
            Debug.Log("Click: เปลี่ยน Cursor");
            isClick = true;
        }
    }

    // เมื่อปล่อยปุ่มเมาส์
    public void OnPointerUp(PointerEventData eventData)
    {
        if (hoverCursor != null)
        {
            Cursor.SetCursor(hoverCursor, hotspot, cursorMode);
        }
    }

    void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}