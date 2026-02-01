using UnityEngine;

public class ScaleShrink : MonoBehaviour
{
    public RectTransform targetImage;

    [Header("Size Settings")]
    public Vector2 startSize = new Vector2(2000f, 2000f);
    public Vector2 targetSize = new Vector2(800f, 800f);
    public float duration = 0.2f;

    private float elapsed = 0f;
    private bool isShrinking = true;

    private void Start()
    {
        if (targetImage != null)
        {
            targetImage.sizeDelta = startSize;
        }
    }

    private void Update()
    {
        if (isShrinking && targetImage != null)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            // ลดขนาดตาม progress
            targetImage.sizeDelta = Vector2.Lerp(startSize, targetSize, progress);

            // เมื่อครบเวลา
            if (progress >= 1f)
            {
                targetImage.sizeDelta = targetSize;
                isShrinking = false;
                Debug.Log("Image is now 800x800!");
            }
        }
    }

    // เรียกเพื่อเริ่มใหม่
    public void ResetAndShrink()
    {
        elapsed = 0f;
        isShrinking = true;
        if (targetImage != null)
        {
            targetImage.sizeDelta = startSize;
        }
    }
}