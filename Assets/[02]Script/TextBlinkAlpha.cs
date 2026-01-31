using UnityEngine;
using TMPro;

public class TextBlinkAlpha : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float blinkSpeed = 1f; // ความเร็วในการกระพลิบ
    public float minAlpha = 0f;   // ค่า Alpha ต่ำสุด (0 = โปร่งใสสนิท)
    public float maxAlpha = 1f;   // ค่า Alpha สูงสุด (1 = ทึบสนิท)
    public bool isBlink = true;
    void Start()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // ใช้ PingPong เพื่อให้ค่าสลับไปมาอัตโนมัติ
        if (isBlink)
        {
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, maxAlpha - minAlpha) + minAlpha;

            Color color = textMesh.color;
            color.a = alpha;
            textMesh.color = color;
        }
    }
}