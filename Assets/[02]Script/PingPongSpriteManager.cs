using UnityEngine;
using System.Collections.Generic;

public class PingPongSpriteManager : MonoBehaviour
{
    [Header("Reference Points")]
    [Tooltip("First reference point for ping-pong movement")]
    public Transform pointA;

    [Tooltip("Second reference point for ping-pong movement")]
    public Transform pointB;

    [Header("Sprite Settings")]
    [Tooltip("List of sprite prefabs to spawn (randomly selected)")]
    public List<GameObject> spritePrefabs = new List<GameObject>();

    [Tooltip("Number of sprites to create")]
    [Range(1, 50)]
    public int spriteCount = 5;

    [Header("Speed Settings")]
    [Tooltip("Minimum random speed")]
    public float minSpeed = 1f;

    [Tooltip("Maximum random speed")]
    public float maxSpeed = 5f;

    private PingPongSprite[] sprites;

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Please assign both Point A and Point B!");
            return;
        }

        if (spritePrefabs == null || spritePrefabs.Count == 0)
        {
            Debug.LogError("Please assign at least one sprite prefab to the list!");
            return;
        }

        SpawnSprites();
    }

    void SpawnSprites()
    {
        sprites = new PingPongSprite[spriteCount];

        for (int i = 0; i < spriteCount; i++)
        {
            // Randomly select a prefab from the list
            GameObject selectedPrefab = spritePrefabs[Random.Range(0, spritePrefabs.Count)];

            // Spawn sprite at random position between the two points
            float randomT = Random.Range(0f, 1f);
            Vector3 spawnPosition = Vector3.Lerp(pointA.position, pointB.position, randomT);

            GameObject spriteObj = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity, transform);
            spriteObj.name = $"PingPongSprite_{i}";

            // Add PingPongSprite component
            PingPongSprite pingPong = spriteObj.AddComponent<PingPongSprite>();

            // Assign random speed
            float randomSpeed = Random.Range(minSpeed, maxSpeed);

            // Initialize the sprite
            pingPong.Initialize(pointA, pointB, randomSpeed);

            sprites[i] = pingPong;
        }
    }

    // Optional: Method to change speed of all sprites
    public void RandomizeAllSpeeds()
    {
        if (sprites != null)
        {
            foreach (var sprite in sprites)
            {
                if (sprite != null)
                {
                    sprite.SetSpeed(Random.Range(minSpeed, maxSpeed));
                }
            }
        }
    }

    // Optional: Method to destroy all sprites
    public void ClearSprites()
    {
        if (sprites != null)
        {
            foreach (var sprite in sprites)
            {
                if (sprite != null)
                {
                    Destroy(sprite.gameObject);
                }
            }
            sprites = null;
        }
    }
}

// Individual sprite controller
public class PingPongSprite : MonoBehaviour
{
    private Transform pointA;
    private Transform pointB;
    private float speed;
    private bool movingToB = true;
    private float journeyProgress = 0f;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void Initialize(Transform a, Transform b, float spd)
    {
        pointA = a;
        pointB = b;
        speed = spd;

        // Randomize starting direction
        movingToB = Random.value > 0.5f;

        // Random starting progress
        journeyProgress = Random.Range(0f, 1f);

        // Set initial flip based on direction
        UpdateSpriteFlip();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        if (pointA == null || pointB == null)
            return;

        float distance = Vector3.Distance(pointA.position, pointB.position);

        if (distance == 0)
            return;

        // Calculate movement step
        float step = (speed * Time.deltaTime) / distance;

        bool previousDirection = movingToB;

        // Update progress
        if (movingToB)
        {
            journeyProgress += step;
            if (journeyProgress >= 1f)
            {
                journeyProgress = 1f;
                movingToB = false;
            }
        }
        else
        {
            journeyProgress -= step;
            if (journeyProgress <= 0f)
            {
                journeyProgress = 0f;
                movingToB = true;
            }
        }

        // Flip sprite when direction changes
        if (previousDirection != movingToB)
        {
            UpdateSpriteFlip();
        }

        // Update position
        transform.position = Vector3.Lerp(pointA.position, pointB.position, journeyProgress);
    }

    void UpdateSpriteFlip()
    {
        if (spriteRenderer != null)
        {
            // Flip sprite based on movement direction
            // Moving to B = face right (no flip), Moving to A = face left (flip)
            spriteRenderer.flipX = !movingToB;
        }
    }

    // Optional: Draw gizmo to show movement path
    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}