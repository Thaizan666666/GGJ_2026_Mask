using UnityEngine;
using UnityEngine.InputSystem;

public enum StageEvent
{
    None = 0,
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
    [SerializeField] private AnimationCurve bobbingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Customer Enter - Bobbing Settings")]
    [SerializeField] private float enterBobbingAmplitude = 0.08f;
    [SerializeField] private float enterBobbingSpeed = 1f;

    [Header("Service Start - Bobbing Settings")]
    [SerializeField] private float serviceStartAmplitude = 0.15f;
    [SerializeField] private float serviceStartSpeed = 1.2f;

    [Header("Service End - Bobbing Settings")]
    [SerializeField] private float serviceEndAmplitude = 0.05f;
    [SerializeField] private float serviceEndSpeed = 0.8f;

    [Header("Customer Exit - Bobbing Settings")]
    [SerializeField] private float exitBobbingAmplitude = 0.08f;
    [SerializeField] private float exitBobbingSpeed = 1f;

    [Header("None/Idle - Bobbing Settings")]
    [SerializeField] private float noneBobbingAmplitude = 0.02f;
    [SerializeField] private float noneBobbingSpeed = 0.5f;

    // Movement tracking variables
    private bool isMoving = false;
    private Vector3 moveStartPosition;
    private Vector3 moveTargetPosition;
    private float moveDuration;
    private float moveElapsedTime;

    // Bobbing tracking variables
    private float bobbingTime;
    private float currentBobbingAmplitude;
    private float currentBobbingSpeed;

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
        bobbingTime += Time.deltaTime * currentBobbingSpeed;

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

    public void SetStagToEndService()
    {

        stageEvent = StageEvent.Service_End;
        SetBobbingForStage(StageEvent.Service_End);
    }

    public void MoveTo(StageEvent stg)
    {
        if (Customer_Object == null)
        {
            Debug.LogWarning("Customer_Object is null!");
            return;
        }

        stageEvent = stg;

        // Set bobbing amplitude and speed based on stage
        SetBobbingForStage(stg);

        switch (stg)
        {
            case StageEvent.Customer_Enter:
                // Move to service point with enter bobbing
                if (ServicePoint != null)
                {
                    StartMovement(ServicePoint.transform.position);
                }
                break;

            case StageEvent.Service_Start:
                // Stay at service point with mid amplitude bobbing
                if (ServicePoint != null)
                {
                    StopMovement();
                    Customer_Object.transform.position = ServicePoint.transform.position;
                }
                break;

            case StageEvent.Service_End:
                // Stay at service point with low amplitude bobbing
                if (ServicePoint != null)
                {
                    StopMovement();
                    Customer_Object.transform.position = ServicePoint.transform.position;
                }
                break;

            case StageEvent.Customer_Exit:
                // Move to exit point with exit bobbing
                if (ExitPoint != null)
                {
                    StartMovement(ExitPoint.transform.position);
                }
                break;

            case StageEvent.WarpToStartPoint:
                // Instantly warp to start point
                if (EnterPoint != null)
                {
                    StopMovement();
                    Customer_Object.transform.position = EnterPoint.transform.position;
                    Debug.Log("Warped to start point");
                }
                break;

            case StageEvent.None:
                // Minimal bobbing at current position
                StopMovement();
                break;
        }
    }

    /// <summary>
    /// Sets the bobbing amplitude and speed based on the current stage
    /// </summary>
    private void SetBobbingForStage(StageEvent stg)
    {
        switch (stg)
        {
            case StageEvent.Customer_Enter:
                SetBobbingAmplitude(enterBobbingAmplitude);
                SetBobbingSpeed(enterBobbingSpeed);
                break;

            case StageEvent.Service_Start:
                SetBobbingAmplitude(serviceStartAmplitude);
                SetBobbingSpeed(serviceStartSpeed);
                break;

            case StageEvent.Service_End:
                SetBobbingAmplitude(serviceEndAmplitude);
                SetBobbingSpeed(serviceEndSpeed);
                break;

            case StageEvent.Customer_Exit:
                SetBobbingAmplitude(exitBobbingAmplitude);
                SetBobbingSpeed(exitBobbingSpeed);
                break;

            case StageEvent.WarpToStartPoint:
            case StageEvent.None:
                SetBobbingAmplitude(noneBobbingAmplitude);
                SetBobbingSpeed(noneBobbingSpeed);
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
    /// Get current bobbing speed
    /// </summary>
    public float GetCurrentBobbingSpeed()
    {
        return currentBobbingSpeed;
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
        currentBobbingSpeed = Mathf.Max(0.1f, speed);
    }

    /// <summary>
    /// Enable or disable bobbing
    /// </summary>
    public void SetBobbingEnabled(bool enabled)
    {
        enableBobbing = enabled;
    }

    /// <summary>
    /// Get the bobbing amplitude for a specific stage
    /// </summary>
    public float GetStageBobbingAmplitude(StageEvent stg)
    {
        switch (stg)
        {
            case StageEvent.Customer_Enter: return enterBobbingAmplitude;
            case StageEvent.Service_Start: return serviceStartAmplitude;
            case StageEvent.Service_End: return serviceEndAmplitude;
            case StageEvent.Customer_Exit: return exitBobbingAmplitude;
            case StageEvent.WarpToStartPoint:
            case StageEvent.None: return noneBobbingAmplitude;
            default: return 0f;
        }
    }

    /// <summary>
    /// Get the bobbing speed for a specific stage
    /// </summary>
    public float GetStageBobbingSpeed(StageEvent stg)
    {
        switch (stg)
        {
            case StageEvent.Customer_Enter: return enterBobbingSpeed;
            case StageEvent.Service_Start: return serviceStartSpeed;
            case StageEvent.Service_End: return serviceEndSpeed;
            case StageEvent.Customer_Exit: return exitBobbingSpeed;
            case StageEvent.WarpToStartPoint:
            case StageEvent.None: return noneBobbingSpeed;
            default: return 1f;
        }
    }

    /// <summary>
    /// Set the bobbing amplitude for a specific stage
    /// </summary>
    public void SetStageBobbingAmplitude(StageEvent stg, float amplitude)
    {
        switch (stg)
        {
            case StageEvent.Customer_Enter: enterBobbingAmplitude = amplitude; break;
            case StageEvent.Service_Start: serviceStartAmplitude = amplitude; break;
            case StageEvent.Service_End: serviceEndAmplitude = amplitude; break;
            case StageEvent.Customer_Exit: exitBobbingAmplitude = amplitude; break;
            case StageEvent.WarpToStartPoint:
            case StageEvent.None: noneBobbingAmplitude = amplitude; break;
        }

        // If we're currently in this stage, update the current values
        if (stageEvent == stg)
        {
            SetBobbingAmplitude(amplitude);
        }
    }

    /// <summary>
    /// Set the bobbing speed for a specific stage
    /// </summary>
    public void SetStageBobbingSpeed(StageEvent stg, float speed)
    {
        switch (stg)
        {
            case StageEvent.Customer_Enter: enterBobbingSpeed = speed; break;
            case StageEvent.Service_Start: serviceStartSpeed = speed; break;
            case StageEvent.Service_End: serviceEndSpeed = speed; break;
            case StageEvent.Customer_Exit: exitBobbingSpeed = speed; break;
            case StageEvent.WarpToStartPoint:
            case StageEvent.None: noneBobbingSpeed = speed; break;
        }

        // If we're currently in this stage, update the current values
        if (stageEvent == stg)
        {
            SetBobbingSpeed(speed);
        }
    }
}