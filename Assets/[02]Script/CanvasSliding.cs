using UnityEngine;
using System.Collections;

public enum SlideDirection
{
    Up,
    Down,
    Left,
    Right,
    Custom
}

public class CanvasSliding : MonoBehaviour
{
    [Header("Slide Settings")]
    [Tooltip("Direction to slide the canvas")]
    public SlideDirection slideDirection = SlideDirection.Up;

    [Tooltip("Custom direction vector (only used when direction is set to Custom)")]
    public Vector2 customDirection = Vector2.up;

    [Tooltip("Duration of the slide animation in seconds")]
    public float slideDuration = 0.5f;

    [Tooltip("Distance to slide (in pixels)")]
    public float slideDistance = 300f;

    [Tooltip("Animation curve for the slide motion")]
    public AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Optional Settings")]
    [Tooltip("Delay before starting the slide animation")]
    public float startDelay = 0f;

    [Tooltip("Should the canvas be hidden at start?")]
    public bool hideOnStart = true;

    [Tooltip("Canvas Group for fade effect (optional)")]
    public CanvasGroup canvasGroup;

    [Tooltip("Fade in while sliding?")]
    public bool fadeWhileSliding = false;

    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isAnimating = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("CanvasSlideUp requires a RectTransform component!");
            enabled = false;
            return;
        }

        // Store the original position
        startPosition = rectTransform.anchoredPosition;

        if (hideOnStart)
        {
            // Move canvas in the opposite direction of slide by slideDistance
            Vector2 offset = GetDirectionVector() * slideDistance;
            rectTransform.anchoredPosition = startPosition - offset;

            if (canvasGroup != null && fadeWhileSliding)
            {
                canvasGroup.alpha = 0f;
            }
        }
    }

    /// <summary>
    /// Get the direction vector based on the current slide direction setting
    /// </summary>
    private Vector2 GetDirectionVector()
    {
        switch (slideDirection)
        {
            case SlideDirection.Up:
                return Vector2.up;
            case SlideDirection.Down:
                return Vector2.down;
            case SlideDirection.Left:
                return Vector2.left;
            case SlideDirection.Right:
                return Vector2.right;
            case SlideDirection.Custom:
                return customDirection.normalized;
            default:
                return Vector2.up;
        }
    }

    /// <summary>
    /// Trigger the slide animation (slides in the set direction)
    /// </summary>
    public void TriggerSlideIn()
    {
        if (!isAnimating)
        {
            StartCoroutine(SlideInCoroutine());
        }
    }

    /// <summary>
    /// Trigger slide out animation (reverse of slide in)
    /// </summary>
    public void TriggerSlideOut()
    {
        if (!isAnimating)
        {
            StartCoroutine(SlideOutCoroutine());
        }
    }

    /// <summary>
    /// Legacy method name for backwards compatibility
    /// </summary>
    public void TriggerSlideUp()
    {
        TriggerSlideIn();
    }

    /// <summary>
    /// Legacy method name for backwards compatibility
    /// </summary>
    public void TriggerSlideDown()
    {
        TriggerSlideOut();
    }

    /// <summary>
    /// Toggle between slide in and slide out
    /// </summary>
    public void ToggleSlide()
    {
        if (!isAnimating)
        {
            // Check if we're at the start position or end position
            float distance = Vector2.Distance(rectTransform.anchoredPosition, startPosition);

            if (distance < 1f) // At start position (slid in)
            {
                StartCoroutine(SlideOutCoroutine());
            }
            else // At end position (slid out)
            {
                StartCoroutine(SlideInCoroutine());
            }
        }
    }

    private IEnumerator SlideInCoroutine()
    {
        isAnimating = true;

        // Wait for start delay
        if (startDelay > 0)
        {
            yield return new WaitForSeconds(startDelay);
        }

        Vector2 fromPosition = rectTransform.anchoredPosition;
        Vector2 toPosition = startPosition;

        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);

            // Apply animation curve
            float curveValue = slideCurve.Evaluate(t);

            // Interpolate position
            rectTransform.anchoredPosition = Vector2.Lerp(fromPosition, toPosition, curveValue);

            // Optional fade
            if (canvasGroup != null && fadeWhileSliding)
            {
                canvasGroup.alpha = curveValue;
            }

            yield return null;
        }

        // Ensure final position is exact
        rectTransform.anchoredPosition = toPosition;

        if (canvasGroup != null && fadeWhileSliding)
        {
            canvasGroup.alpha = 1f;
        }

        isAnimating = false;
    }

    private IEnumerator SlideOutCoroutine()
    {
        isAnimating = true;

        Vector2 fromPosition = rectTransform.anchoredPosition;
        Vector2 offset = GetDirectionVector() * slideDistance;
        Vector2 toPosition = startPosition - offset;

        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);

            // Apply animation curve
            float curveValue = slideCurve.Evaluate(t);

            // Interpolate position
            rectTransform.anchoredPosition = Vector2.Lerp(fromPosition, toPosition, curveValue);

            // Optional fade
            if (canvasGroup != null && fadeWhileSliding)
            {
                canvasGroup.alpha = 1f - curveValue;
            }

            yield return null;
        }

        // Ensure final position is exact
        rectTransform.anchoredPosition = toPosition;

        if (canvasGroup != null && fadeWhileSliding)
        {
            canvasGroup.alpha = 0f;
        }

        isAnimating = false;
    }

    /// <summary>
    /// Reset canvas to its hidden position instantly
    /// </summary>
    public void ResetToHidden()
    {
        StopAllCoroutines();
        isAnimating = false;
        Vector2 offset = GetDirectionVector() * slideDistance;
        rectTransform.anchoredPosition = startPosition - offset;

        if (canvasGroup != null && fadeWhileSliding)
        {
            canvasGroup.alpha = 0f;
        }
    }

    /// <summary>
    /// Reset canvas to its visible position instantly
    /// </summary>
    public void ResetToVisible()
    {
        StopAllCoroutines();
        isAnimating = false;
        rectTransform.anchoredPosition = startPosition;

        if (canvasGroup != null && fadeWhileSliding)
        {
            canvasGroup.alpha = 1f;
        }
    }
}