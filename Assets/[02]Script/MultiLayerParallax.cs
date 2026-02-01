using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    public GameObject layerObject;
    [Range(0f, 10f)]
    public float speedMultiplier = 1f;
    [Tooltip("Leave empty to use global direction")]
    public Vector2 customDirection = Vector2.zero;
}

public class MultiLayerParallax : MonoBehaviour
{
    [Header("Global Settings")]
    [Tooltip("Base speed for all layers")]
    public float baseSpeed = 2f;

    [Tooltip("Global direction (can be overridden per layer)")]
    public Vector2 globalDirection = Vector2.left;

    [Header("Layers")]
    [Tooltip("Add your background layers here, front to back")]
    public ParallaxLayer[] layers;

    private ParallaxBackground[] parallaxScripts;

    void Start()
    {
        SetupLayers();
    }

    void SetupLayers()
    {
        parallaxScripts = new ParallaxBackground[layers.Length];

        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].layerObject != null)
            {
                // Get or add ParallaxBackground component
                ParallaxBackground pb = layers[i].layerObject.GetComponent<ParallaxBackground>();
                if (pb == null)
                {
                    pb = layers[i].layerObject.AddComponent<ParallaxBackground>();
                }

                // Set direction
                if (layers[i].customDirection != Vector2.zero)
                {
                    pb.parallaxDirection = layers[i].customDirection;
                }
                else
                {
                    pb.parallaxDirection = globalDirection;
                }

                // Set speed
                pb.parallaxSpeed = baseSpeed * layers[i].speedMultiplier;

                parallaxScripts[i] = pb;
            }
        }
    }

    void Update()
    {
        // Update all layers if global settings change
        UpdateLayers();
    }

    void UpdateLayers()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (parallaxScripts[i] != null)
            {
                // Update direction if using global
                if (layers[i].customDirection == Vector2.zero)
                {
                    parallaxScripts[i].parallaxDirection = globalDirection;
                }

                // Update speed
                parallaxScripts[i].parallaxSpeed = baseSpeed * layers[i].speedMultiplier;
            }
        }
    }

    // Public methods to control parallax at runtime
    public void SetBaseSpeed(float speed)
    {
        baseSpeed = speed;
    }

    public void SetGlobalDirection(Vector2 direction)
    {
        globalDirection = direction;
    }

    public void SetLayerSpeed(int layerIndex, float multiplier)
    {
        if (layerIndex >= 0 && layerIndex < layers.Length)
        {
            layers[layerIndex].speedMultiplier = multiplier;
        }
    }

    public void PauseParallax()
    {
        foreach (var pb in parallaxScripts)
        {
            if (pb != null)
            {
                pb.enabled = false;
            }
        }
    }

    public void ResumeParallax()
    {
        foreach (var pb in parallaxScripts)
        {
            if (pb != null)
            {
                pb.enabled = true;
            }
        }
    }
}