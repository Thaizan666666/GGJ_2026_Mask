using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [Header("Target Components")]
    public Image targetImage;
    public Button targetButton;

    [Header("Fade Settings")]
    public float fadeSpeed = 1f;
    public float delay = 1f;
    private float count;
    public bool Gotostart = false;

    private Color imageColor;
    private Color buttonColor;
    private Image buttonImage;

    private void Start()
    {
        if (targetImage != null)
        {
            imageColor = targetImage.color;
            imageColor.a = 0f; // เริ่มที่โปร่งใส
            targetImage.color = imageColor;
        }

        if (targetButton != null)
        {
            buttonImage = targetButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonColor = buttonImage.color;
                buttonColor.a = 0f; // เริ่มที่โปร่งใส
                buttonImage.color = buttonColor;

                // ปิดปุ่มตอนเริ่มต้น
                targetButton.interactable = false;
            }
        }
    }

    private void Update()
    {
        if (count <= delay)
        {
            count += Time.deltaTime;
            Gotostart = true;
        }
        if (Gotostart)
        {
            // Fade In Image
            if (targetImage != null && imageColor.a < 1f)
            {
                imageColor.a += fadeSpeed * Time.deltaTime;
                imageColor.a = Mathf.Clamp01(imageColor.a);
                targetImage.color = imageColor;
            }

            // Fade In Button
            if (buttonImage != null && buttonColor.a < 1f)
            {
                buttonColor.a += fadeSpeed * Time.deltaTime;
                buttonColor.a = Mathf.Clamp01(buttonColor.a);
                buttonImage.color = buttonColor;

                // เปิดการกดปุ่มเมื่อ Fade In เสร็จ
                if (buttonColor.a >= 1f)
                {
                    targetButton.interactable = true;
                    Debug.Log("Fade In complete!");
                    this.enabled = false;
                }
            }
        }
    }
}