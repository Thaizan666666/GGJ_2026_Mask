using UnityEngine;
using UnityEngine.UI;

public class JarOfDoh : MonoBehaviour
{
    public MainGameSystem MGS;
    public ImageSequenceButton ISB;
    public bool CanClick = true;

    [Header("Doh Barrel Settings")]
    //public int currentDohBarrel = 3; // Starting amount
    public Sprite[] dohSprites; // Array of sprites for different barrel levels
    public Sprite[] dohSpritesHi;
    private Image jarImage;
    private Button jarButton;

    private void Start()
    {
        // Get the Image and Button components
        jarImage = GetComponent<Image>();
        jarButton = GetComponent<Button>();

        // Add click listener
        if (jarButton != null)
        {
            jarButton.onClick.AddListener(OnJarClicked);
        }

        // Set initial sprite
        UpdateSprite();
    }

    private void Update()
    {
        if (ISB.clickCount < 3)
        {
            CanClick = false;
        }
        if (ISB.clickCount > 3)
        {
            CanClick = true;
        }

        // Update button interactability
        if (jarButton != null)
        {
            jarButton.interactable = CanClick && MGS.currentDohBarral > 0;
        }
    }

    private void OnJarClicked()
    {
        // Check if we can click
        if (!CanClick || MGS.currentDohBarral <= 0)
            return;

        // Decrease the barrel count
        MGS.currentDohBarral--;

        // Update the sprite to reflect current barrel level
        UpdateSprite();
        ISB.ResetSequence();

        // If barrel is empty, disable this UI element
        if (MGS.currentDohBarral <= 0)
        {
            gameObject.SetActive(false);
            jarButton.interactable = false;
            
            // Or alternatively just disable the button:
            // jarButton.interactable = false;
        }
    }

    private void UpdateSprite()
    {
        // Change sprite based on currentDohBarrel
        if (jarImage != null && dohSprites != null && dohSprites.Length > 0)
        {
            // Clamp the index to valid range
            int spriteIndex = Mathf.Clamp(MGS.currentDohBarral, 0, dohSprites.Length - 1);
            jarImage.sprite = dohSprites[spriteIndex];
        }
        if (jarButton != null && dohSpritesHi != null && dohSpritesHi.Length > 0)
        {
            // Clamp the index to valid range
            int spriteIndex = Mathf.Clamp(MGS.currentDohBarral, 0, dohSpritesHi.Length - 1);

            // Create a temporary SpriteState, modify it, then assign it back
            SpriteState spriteState = jarButton.spriteState;
            spriteState.highlightedSprite = dohSpritesHi[spriteIndex];
            jarButton.spriteState = spriteState;
        }
    }
}