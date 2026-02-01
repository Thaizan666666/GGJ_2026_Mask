using UnityEngine;

public class Feater_DropColor : MonoBehaviour
{
    public GameObject targetObject;
    public float fadeSpeed = 0.5f; // ความเร็วในการเพิ่ม Alpha (ปรับได้)
    public float maxAlpha = 1f; // ค่า max ต้องอยู่ระหว่าง 0-1

    private Renderer targetRenderer;
    private Color currentColor;

    private void Start()
    {
        if (targetObject != null)
        {
            targetRenderer = targetObject.GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                currentColor = targetRenderer.material.color;
                currentColor.a = 0f;
                targetRenderer.material.color = currentColor;
            }
        }
    }

    private void Update()
    {
        if (targetRenderer != null && currentColor.a < maxAlpha) // เปลี่ยนจาก <= เป็น 
        {
            // ค่อยๆ เพิ่มค่า Alpha
            currentColor.a += fadeSpeed * Time.deltaTime;

            // จำกัดไม่ให้เกิน maxAlpha
            currentColor.a = Mathf.Clamp(currentColor.a, 0f, maxAlpha);

            // Set สีใหม่
            targetRenderer.material.color = currentColor;
        }
    }
}