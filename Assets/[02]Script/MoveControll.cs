using UnityEngine;
using UnityEngine.InputSystem;

public enum StageEvent
{
    None,
    Customer_Enter,
    Customer_Exit,
    Service_Start,
    Service_End,
    WarpToStartPoint
}

public class MoveControll : MonoBehaviour
{
    public GameObject Customer_Object;

    [Header("")]
    public GameObject EnterPoint;
    public GameObject ServicePoint;
    public GameObject ExitPoint;
    public StageEvent stageEvent = default;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Bobbing Animation Settings")]
    [SerializeField] private bool enableBobbing = true;
    [SerializeField] private float bobbingSpeed = 1f;
    [SerializeField] private AnimationCurve bobbingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Stage-Specific Bobbing Amplitudes")]
    [SerializeField] private float enterBobbingAmplitude = 0.08f; // While entering
    [SerializeField] private float serviceStartAmplitude = 0.15f; // Mid amplitude
    [SerializeField] private float serviceEndAmplitude = 0.05f; // Low amplitude
    [SerializeField] private float exitBobbingAmplitude = 0.08f; // While exiting
    [SerializeField] private float noneBobbingAmplitude = 0.02f; // Minimal bobbing when idle

    // Movement tracking variables
    private bool isMoving = false;
    private Vector3 moveStartPosition;
    private Vector3 moveTargetPosition;
    private float moveDuration;
    private float moveElapsedTime;

    // Bobbing tracking variables
    private float bobbingTime;
    private float currentBobbingAmplitude;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MoveTo(StageEvent.Customer_Enter);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveTo(StageEvent.Service_Start);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            MoveTo(StageEvent.Service_End);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            MoveTo(StageEvent.Customer_Exit);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            MoveTo(StageEvent.WarpToStartPoint);
        }

        if (Customer_Object == null) return;

        // Handle movement
        if (isMoving)
        {
            UpdateMovement();
        }

        // Always handle bobbing (if enabled)
        if (enableBobbing)
        {
            UpdateBobbing();
        }
    }

    private void UpdateMovement()
    {
        moveElapsedTime += Time.deltaTime;
        float normalizedTime = Mathf.Clamp01(moveElapsedTime / moveDuration);

        // Apply animation curve
        float curvedTime = movementCurve.Evaluate(normalizedTime);

        // Calculate base position from lerp
        Vector3 basePosition = Vector3.Lerp(moveStartPosition, moveTargetPosition, curvedTime);

        // Apply bobbing offset to the moving position
        float bobbingOffset = CalculateBobbingOffset();
        Customer_Object.transform.position = basePosition + new Vector3(0, bobbingOffset, 0);

        // Check if movement is complete
        if (normalizedTime >= 1f)
        {
            isMoving = false;
            OnMovementComplete();
        }
    }

    private void UpdateBobbing()
    {
        bobbingTime += Time.deltaTime * bobbingSpeed;

        // Only apply bobbing when not moving
        if (!isMoving)
        {
            float bobbingOffset = CalculateBobbingOffset();

            // Get base position based on current stage
            Vector3 basePosition = GetCurrentBasePosition();
            Customer_Object.transform.position = basePosition + new Vector3(0, bobbingOffset, 0);
        }
    }

    private float CalculateBobbingOffset()
    {
        // Create smooth bobbing motion using sine wave
        float normalizedSine = (Mathf.Sin(bobbingTime * Mathf.PI * 2) + 1) / 2; // 0 to 1
        float curvedBobbing = bobbingCurve.Evaluate(normalizedSine);
        float bobbingOffset = (curvedBobbing - 0.5f) * 2 * currentBobbingAmplitude;

        return bobbingOffset;
    }

    private Vector3 GetCurrentBasePosition()
    {
        // Return the appropriate base position based on current stage
        switch (stageEvent)
        {
            case StageEvent.Customer_Enter:
                return ServicePoint != null ? ServicePoint.transform.position : Vector3.zero;

            case StageEvent.Service_Start:
            case StageEvent.Service_End:
                return ServicePoint != null ? ServicePoint.transform.position : Vector3.zero;

            case StageEvent.Customer_Exit:
                return ExitPoint != null ? ExitPoint.transform.position : Vector3.zero;

            case StageEvent.None:
            case StageEvent.WarpToStartPoint:
                return EnterPoint != null ? EnterPoint.transform.position : Vector3.zero;

            default:
                return Customer_Object.transform.position;
        }
    }

    private void OnMovementComplete()
    {
        Debug.Log($"Movement complete for stage: {stageEvent}");
    }

    public void MoveTo(StageEvent stg)
    {
        if (Customer_Object == null)
        {
            Debug.LogWarning("Customer_Object is null!");
            return;
        }

        stageEvent = stg;

        switch (stg)
        {
            case StageEvent.Customer_Enter:
                // Move to service point with enter bobbing
                if (ServicePoint != null)
                {
                    SetBobbingAmplitude(enterBobbingAmplitude);
                    StartMovement(ServicePoint.transform.position);
                }
                break;

            case StageEvent.Service_Start:
                // Stay at service point with mid amplitude bobbing
                if (ServicePoint != null)
                {
                    SetBobbingAmplitude(serviceStartAmplitude);
                    StopMovement();
                    Customer_Object.transform.position = ServicePoint.transform.position;
                }
                break;

            case StageEvent.Service_End:
                // Stay at service point with low amplitude bobbing
                if (ServicePoint != null)
                {
                    SetBobbingAmplitude(serviceEndAmplitude);
                    StopMovement();
                    Customer_Object.transform.position = ServicePoint.transform.position;
                }
                break;

            case StageEvent.Customer_Exit:
                // Move to exit point with exit bobbing
                if (ExitPoint != null)
                {
                    SetBobbingAmplitude(exitBobbingAmplitude);
                    StartMovement(ExitPoint.transform.position);
                }
                break;

            case StageEvent.WarpToStartPoint:
                // Instantly warp to start point
                if (EnterPoint != null)
                {
                    SetBobbingAmplitude(noneBobbingAmplitude);
                    StopMovement();
                    Customer_Object.transform.position = EnterPoint.transform.position;
                    Debug.Log("Warped to start point");
                }
                break;

            case StageEvent.None:
                // Minimal bobbing at current position
                SetBobbingAmplitude(noneBobbingAmplitude);
                StopMovement();
                break;
        }
    }

    private void StartMovement(Vector3 targetPosition)
    {
        moveStartPosition = Customer_Object.transform.position;
        moveTargetPosition = targetPosition;

        float distance = Vector3.Distance(moveStartPosition, moveTargetPosition);
        moveDuration = distance / moveSpeed;
        moveElapsedTime = 0f;

        isMoving = true;
    }

    private void StopMovement()
    {
        isMoving = false;
    }

    public void StopAllMovement()
    {
        isMoving = false;
        // Note: Bobbing continues even when movement stops
    }

    // PUBLIC METHODS TO TRACK MOVEMENT STATE

    /// <summary>
    /// Check if the customer has finished moving to their destination
    /// </summary>
    public bool IsMovementComplete()
    {
        return !isMoving;
    }

    /// <summary>
    /// Check if the customer is currently moving
    /// </summary>
    public bool IsCurrentlyMoving()
    {
        return isMoving;
    }

    /// <summary>
    /// Check if the customer is currently bobbing
    /// </summary>
    public bool IsCurrentlyBobbing()
    {
        return enableBobbing;
    }

    /// <summary>
    /// Get movement progression from 0 to 1
    /// </summary>
    public float GetMovementProgress()
    {
        if (!isMoving) return 1f;
        return Mathf.Clamp01(moveElapsedTime / moveDuration);
    }

    /// <summary>
    /// Get remaining distance to target
    /// </summary>
    public float GetRemainingDistance()
    {
        if (!isMoving || Customer_Object == null) return 0f;
        return Vector3.Distance(Customer_Object.transform.position, moveTargetPosition);
    }

    /// <summary>
    /// Get remaining time for movement in seconds
    /// </summary>
    public float GetRemainingTime()
    {
        if (!isMoving) return 0f;
        return moveDuration - moveElapsedTime;
    }

    /// <summary>
    /// Get current bobbing phase (0 to 1, cycles continuously)
    /// </summary>
    public float GetBobbingPhase()
    {
        return (bobbingTime % 1f);
    }

    /// <summary>
    /// Get current bobbing amplitude
    /// </summary>
    public float GetCurrentBobbingAmplitude()
    {
        return currentBobbingAmplitude;
    }

    /// <summary>
    /// Force complete current movement instantly
    /// </summary>
    public void ForceCompleteMovement()
    {
        if (isMoving && Customer_Object != null)
        {
            Customer_Object.transform.position = moveTargetPosition;
            isMoving = false;
            OnMovementComplete();
        }
    }

    /// <summary>
    /// Set movement speed at runtime
    /// </summary>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = Mathf.Max(0.1f, speed);

        // Recalculate duration if currently moving
        if (isMoving)
        {
            float distance = Vector3.Distance(moveStartPosition, moveTargetPosition);
            moveDuration = distance / moveSpeed;
        }
    }

    /// <summary>
    /// Set bobbing amplitude at runtime
    /// </summary>
    public void SetBobbingAmplitude(float amplitude)
    {
        currentBobbingAmplitude = amplitude;
    }

    /// <summary>
    /// Set bobbing speed at runtime
    /// </summary>
    public void SetBobbingSpeed(float speed)
    {
        bobbingSpeed = Mathf.Max(0.1f, speed);
    }

    /// <summary>
    /// Enable or disable bobbing
    /// </summary>
    public void SetBobbingEnabled(bool enabled)
    {
        enableBobbing = enabled;
    }
}