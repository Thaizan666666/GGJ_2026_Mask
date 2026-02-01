using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("Speed multiplier for the parallax effect")]
    public float parallaxSpeed = 1f;

    [Tooltip("Direction of parallax movement (normalized automatically)")]
    public Vector2 parallaxDirection = Vector2.left;

    [Header("Auto-setup")]
    [Tooltip("Automatically duplicate sprite for seamless loop")]
    public bool autoSetup = true;

    [Header("References")]
    [Tooltip("Assign if not using auto-setup")]
    public SpriteRenderer spriteRenderer;

    private Transform cam;
    private Vector3 startPosition;
    private float spriteWidth;
    private float spriteHeight;
    private Vector2 normalizedDirection;
    private GameObject clone;

    void Start()
    {
        cam = Camera.main.transform;
        startPosition = transform.position;

        // Get sprite renderer if not assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("ParallaxBackground requires a SpriteRenderer component!");
            return;
        }

        // Calculate sprite dimensions based on actual size in world space
        Bounds bounds = spriteRenderer.bounds;
        spriteWidth = bounds.size.x;
        spriteHeight = bounds.size.y;

        // Normalize direction
        normalizedDirection = parallaxDirection.normalized;

        // Auto-setup: Create clone for seamless looping
        if (autoSetup)
        {
            CreateClone();
        }
    }

    void CreateClone()
    {
        // Create a duplicate sprite for seamless looping
        clone = new GameObject(gameObject.name + "_Clone");
        clone.transform.parent = transform.parent;

        SpriteRenderer cloneSR = clone.AddComponent<SpriteRenderer>();
        cloneSR.sprite = spriteRenderer.sprite;
        cloneSR.sortingLayerName = spriteRenderer.sortingLayerName;
        cloneSR.sortingOrder = spriteRenderer.sortingOrder;
        cloneSR.material = spriteRenderer.material;

        // Copy transform properties
        clone.transform.localScale = transform.localScale;
        clone.transform.rotation = transform.rotation;

        // Position clone based on direction
        Vector3 offset;
        if (Mathf.Abs(normalizedDirection.x) > Mathf.Abs(normalizedDirection.y))
        {
            // Horizontal scrolling
            offset = new Vector3(spriteWidth * Mathf.Sign(normalizedDirection.x), 0, 0);
        }
        else
        {
            // Vertical scrolling
            offset = new Vector3(0, spriteHeight * Mathf.Sign(normalizedDirection.y), 0);
        }

        clone.transform.position = transform.position + offset;
    }

    void Update()
    {
        // Normalize direction in case it changed in inspector
        normalizedDirection = parallaxDirection.normalized;

        // Move the background
        Vector3 movement = new Vector3(normalizedDirection.x, normalizedDirection.y, 0)
                          * parallaxSpeed * Time.deltaTime;
        transform.position += movement;

        if (clone != null)
        {
            clone.transform.position += movement;
        }

        // Check for looping - reset position when sprite has moved one full width/height
        CheckLoop();
    }

    void CheckLoop()
    {
        float distanceMoved;
        float resetDistance;

        if (Mathf.Abs(normalizedDirection.x) > Mathf.Abs(normalizedDirection.y))
        {
            // Horizontal scrolling
            distanceMoved = transform.position.x - startPosition.x;
            resetDistance = spriteWidth;

            if (normalizedDirection.x < 0) // Moving left
            {
                if (distanceMoved <= -resetDistance)
                {
                    transform.position = new Vector3(
                        startPosition.x,
                        transform.position.y,
                        transform.position.z
                    );

                    if (clone != null)
                    {
                        clone.transform.position = new Vector3(
                            startPosition.x + spriteWidth,
                            clone.transform.position.y,
                            clone.transform.position.z
                        );
                    }
                }
            }
            else // Moving right
            {
                if (distanceMoved >= resetDistance)
                {
                    transform.position = new Vector3(
                        startPosition.x,
                        transform.position.y,
                        transform.position.z
                    );

                    if (clone != null)
                    {
                        clone.transform.position = new Vector3(
                            startPosition.x - spriteWidth,
                            clone.transform.position.y,
                            clone.transform.position.z
                        );
                    }
                }
            }
        }
        else
        {
            // Vertical scrolling
            distanceMoved = transform.position.y - startPosition.y;
            resetDistance = spriteHeight;

            if (normalizedDirection.y < 0) // Moving down
            {
                if (distanceMoved <= -resetDistance)
                {
                    transform.position = new Vector3(
                        transform.position.x,
                        startPosition.y,
                        transform.position.z
                    );

                    if (clone != null)
                    {
                        clone.transform.position = new Vector3(
                            clone.transform.position.x,
                            startPosition.y + spriteHeight,
                            clone.transform.position.z
                        );
                    }
                }
            }
            else // Moving up
            {
                if (distanceMoved >= resetDistance)
                {
                    transform.position = new Vector3(
                        transform.position.x,
                        startPosition.y,
                        transform.position.z
                    );

                    if (clone != null)
                    {
                        clone.transform.position = new Vector3(
                            clone.transform.position.x,
                            startPosition.y - spriteHeight,
                            clone.transform.position.z
                        );
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        // Clean up clone when destroyed
        if (clone != null)
        {
            Destroy(clone);
        }
    }
}