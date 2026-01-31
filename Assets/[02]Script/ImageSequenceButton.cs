using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ImageSequenceButton : MonoBehaviour
{
    [Header("References")]
    public Image[] images; // ใส่ 4 รูปเรียงจากบนลงล่าง (0=บน, 3=ล่าง)
    public Button mainButton;
    public TextMeshProUGUI debugText; // แสดงสถานะ (ถ้ามี)

    [Header("Sprites")]
    public Sprite whiteSprite; // Sprite สีขาว
    public Sprite blackSprite; // Sprite สีดำ

    [Header("Settings")]
    public float countdownTime = 5f;
    public bool useSprites = true; // เลือกใช้ Sprite หรือ Color

    [Header("Colors (ถ้าไม่ใช้ Sprite)")]
    public Color whiteColor = Color.white;
    public Color blackColor = Color.black;

    private int currentIndex = 3; // เริ่มจากล่างสุด
    private bool isReady = false;
    private int clickCount = 0;

    void Start()
    {
        // ตั้งค่าปุ่ม
        if (mainButton != null)
        {
            mainButton.onClick.AddListener(OnButtonClick);
            mainButton.interactable = false;
        }

        // เริ่มต้นทุกรูปเป็นสีขาว/Sprite ขาว
        SetAllImagesWhite();

        // เริ่มนับถอยหลัง
        StartCoroutine(CountdownAndTurnBlack());
    }

    IEnumerator CountdownAndTurnBlack()
    {
        float timeLeft = countdownTime;

        // นับถอยหลัง
        while (timeLeft > 0)
        {
            if (debugText != null)
                debugText.text = $"เริ่มใน: {timeLeft:F1}s";

            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
        }

        // เปลี่ยนทุกรูปเป็นสีดำ/Sprite ดำ
        SetAllImagesBlack();

        // เปิดใช้งานปุ่ม
        if (mainButton != null)
            mainButton.interactable = true;

        if (debugText != null)
            debugText.text = "กดปุ่มได้แล้ว!";

        isReady = true;
        Debug.Log("พร้อมกดปุ่มแล้ว!");
    }

    void OnButtonClick()
    {
        if (!isReady) return;

        // ตรวจสอบว่ายังมีรูปที่ต้องเปลี่ยนอีกไหม
        if (currentIndex >= 0)
        {
            clickCount++;

            // เปลี่ยนรูปปัจจุบันเป็นสีขาว/Sprite ขาว
            SetImageWhite(images[currentIndex]);

            if (debugText != null)
                debugText.text = $"กดครั้งที่ {clickCount}: รูปที่ {currentIndex + 1} เป็นสีขาว";

            Debug.Log($"เปลี่ยนรูปที่ {currentIndex + 1} เป็นสีขาว");

            // ลดค่า index เพื่อไปรูปถัดไป
            currentIndex--;

            // ถ้าเปลี่ยนครบทุกรูปแล้ว
            if (currentIndex < 0)
            {
                if (debugText != null)
                    debugText.text = "เสร็จสิ้น! กดครบทั้งหมดแล้ว";

                Debug.Log("เปลี่ยนครบทุกรูปแล้ว!");
                mainButton.interactable = false;
            }
        }
    }

    // ฟังก์ชันช่วยเปลี่ยนรูปเดียวเป็นสีขาว/Sprite ขาว
    void SetImageWhite(Image img)
    {
        if (img == null) return;

        if (useSprites && whiteSprite != null)
        {
            img.sprite = whiteSprite;
        }
        else
        {
            img.color = whiteColor;
        }
    }

    // ฟังก์ชันช่วยเปลี่ยนรูปเดียวเป็นสีดำ/Sprite ดำ
    void SetImageBlack(Image img)
    {
        if (img == null) return;

        if (useSprites && blackSprite != null)
        {
            img.sprite = blackSprite;
        }
        else
        {
            img.color = blackColor;
        }
    }

    // ฟังก์ชันเปลี่ยนทุกรูปเป็นสีขาว/Sprite ขาว
    void SetAllImagesWhite()
    {
        foreach (Image img in images)
        {
            SetImageWhite(img);
        }
    }

    // ฟังก์ชันเปลี่ยนทุกรูปเป็นสีดำ/Sprite ดำ
    void SetAllImagesBlack()
    {
        foreach (Image img in images)
        {
            SetImageBlack(img);
        }
    }

    // ฟังก์ชันรีเซ็ต (เรียกจากปุ่มอื่นได้)
    public void ResetSequence()
    {
        StopAllCoroutines();
        currentIndex = 3;
        isReady = false;
        clickCount = 0;

        SetAllImagesWhite();

        if (mainButton != null)
            mainButton.interactable = false;

        if (debugText != null)
            debugText.text = "";

        StartCoroutine(CountdownAndTurnBlack());
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            ResetSequence();
        }
    }
}