using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageSequenceButton : MonoBehaviour
{
    [Header("References")]
    public Image[] images; // ใส่ 4 รูปเรียงจากบนลงล่าง (0=บน, 3=ล่าง)
    public Button mainButton;
    public TextMeshProUGUI debugText; // แสดงสถานะ (ถ้ามี)
    public MainGameSystem mainGameSystem; // อ้างอิงถึงสคริปต์หลัก (ถ้ามี)

    [Header("Sprites")]
    public Sprite whiteSprite; // Sprite สีขาว
    public Sprite blackSprite; // Sprite สีดำ

    [Header("Settings")]
    public float countdownTime = 5f;
    public bool useSprites = true; // เลือกใช้ Sprite หรือ Color

    [Header("Colors (ถ้าไม่ใช้ Sprite)")]
    public Color whiteColor = Color.white;
    public Color blackColor = Color.black;

    // Time tracking variables
    private float countdownTimer = 0f;
    private bool isCountingDown = false;
    private bool isReady = false;

    private int currentIndex = 3; // เริ่มจากล่างสุด
    public int clickCount = 0;

    // Public properties for tracking
    public float TimeRemaining => Mathf.Max(0, countdownTimer);
    public float TimeElapsed => countdownTime - TimeRemaining;
    public float ProgressPercent => (TimeElapsed / countdownTime) * 100f;
    public bool IsCountingDown => isCountingDown;
    public bool IsReady => isReady;
    public int ClickCount => clickCount;
    public int RemainingClicks => currentIndex + 1;

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
        StartCountdown();
    }

    void Update()
    {
        // Handle countdown
        if (isCountingDown)
        {
            countdownTimer -= Time.deltaTime;

            // Update debug text
            if (debugText != null)
            {
                debugText.text = $"เริ่มใน: {countdownTimer:F1}s ({ProgressPercent:F0}%)";
            }

            // Check if countdown is complete
            if (countdownTimer <= 0)
            {
                OnCountdownComplete();
            }
        }

#if UNITY_EDITOR
        // Reset key
        if (Input.GetKeyDown(KeyCode.F))
        {
            ResetSequence();
        }
#endif

    }

    void StartCountdown()
    {
        countdownTimer = countdownTime;
        isCountingDown = true;
        isReady = false;
        Debug.Log($"Countdown started: {countdownTime} seconds");
    }

    void OnCountdownComplete()
    {
        isCountingDown = false;
        isReady = true;

        // เปลี่ยนทุกรูปเป็นสีดำ/Sprite ดำ
        SetAllImagesBlack();

        // เปิดใช้งานปุ่ม
        if (mainButton != null)
            mainButton.interactable = true;

        if (debugText != null)
            debugText.text = "กดปุ่มได้แล้ว!";

        Debug.Log("พร้อมกดปุ่มแล้ว!");
    }

    void OnButtonClick()
    {
        if (!isReady) return;

        if (mainGameSystem != null)
        {
            // ตรวจสอบว่ายังมีรูปที่ต้องเปลี่ยนอีกไหม
            if (currentIndex >= 0 && !mainGameSystem.IsHaveDoh)
            {
                clickCount++;
                mainGameSystem.EnableBTNTopping(true);
                mainGameSystem.IsHaveDoh = true;
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
                    isReady = false;
                }
            }
        }
        else {
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
                    isReady = false;
                }
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
        currentIndex = 3;
        clickCount = 0;

        SetAllImagesWhite();

        if (mainButton != null)
            mainButton.interactable = false;

        if (debugText != null)
            debugText.text = "";

        StartCountdown();
        Debug.Log("Sequence reset!");
    }

    // Helper method to get detailed time info
    public string GetTimeInfo()
    {
        return $"Time Remaining: {TimeRemaining:F2}s | Elapsed: {TimeElapsed:F2}s | Progress: {ProgressPercent:F1}%";
    }
}