using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NewCharacterMaker : MonoBehaviour
{
    [HideInInspector]
    public E_CharacterType currentCharacterType;
    [Header("CharacterDB")]
    public SO_CharacterSetup characterSetup;

    [Header("SpriteRenderer Socket")]
    public SpriteRenderer CharacterSprite;
    public SpriteRenderer MaskSprite;
    public SpriteRenderer AccesorySprite;
    public SpriteRenderer TailSprite;
    public SpriteRenderer FlameSprite;
    private bool isFemale;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentCharacterType = E_CharacterType.Normal;
            SetupNormalCharacter();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            currentCharacterType = E_CharacterType.Anamoly;
            SetupAnomolyCharacter();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            SwitcthToWetSprite();
        }
    }

    [ContextMenu("Setup Normal Character")]
    public void SetupNormalCharacter()
    {
        ResetCharacterSprites();

        // Pick a gendered base character and apply its sprite + normal accessory
        NormalCharacter normalCharacter = PickGenderedNormalCharacter();
        ApplyNormalCharacter(normalCharacter);
    }

    [ContextMenu("Setup Anomoly Character")]
    public void SetupAnomolyCharacter()
    {
        ResetCharacterSprites();

        // Decide gender once so both the base and anomaly layers share the same gender
        isFemale = Random.Range(0, 2) == 0;

        // Apply the base human look first, then layer the anomaly accessory on top
        NormalCharacter normalCharacter = PickGenderedNormalCharacter(isFemale);
        ApplyNormalCharacter(normalCharacter);

        // Pick the gendered accessory bag from the SO
        AnomolyAccessory anomolyAccessory = isFemale
            ? characterSetup.anomolyFemaleAccessory
            : characterSetup.anomolyMaleAccessory;
        ApplyAnomolyAccessory(anomolyAccessory);
    }

    [ContextMenu("Switch To Wet Sprite")]
    public void SwitcthToWetSprite()
    {
        if (CharacterSprite.sprite == null)
        {
            Debug.LogWarning("SwitchToWetSprite: CharacterSprite is null, cannot switch to wet sprite.");
            return;
        }

        switch (currentCharacterType)
        {
            case E_CharacterType.Normal:
                // Look up the wet sprite that corresponds to the current dry sprite (the key)
                // CharacterSprites is a Dictionary<DrySprite, WetSprite>
                NormalCharacter normalCharacter = isFemale
                    ? characterSetup.normalFemale
                    : characterSetup.normalMale;

                if (normalCharacter.CharacterSprites.TryGetValue(CharacterSprite.sprite, out Sprite wetSprite))
                {
                    CharacterSprite.sprite = wetSprite;
                    Debug.Log("SwitchToWetSprite: Successfully switched to wet sprite.");
                }
                else
                    Debug.LogWarning("SwitchToWetSprite: No wet sprite found for the current character sprite.");
                break;

            case E_CharacterType.Anamoly:
                // Anomaly characters transform fully — clear all accessories and show a random anomaly sprite
                ResetCharacterSprites();
                CharacterSprite.sprite = characterSetup.AnomolySprite.RandomValue();
                break;
        }
    }

    /// <summary>
    /// Clears all sprite renderers back to null before building a new character,
    /// preventing leftover sprites from a previous setup from bleeding through.
    /// </summary>
    public void ResetCharacterSprites()
    {
        CharacterSprite.sprite = null;
        MaskSprite.sprite = null;
        AccesorySprite.sprite = null;
        AccesorySprite.sortingOrder = 2; // Reset sorting order in case it was changed by an anomaly accessory
        TailSprite.sprite = null;
        FlameSprite.sprite = null;
    }

    /// <summary>
    /// Randomly decides gender internally, then returns the matching NormalCharacter data.
    /// Use this overload when the caller doesn't need to know the gender afterwards.
    /// </summary>
    private NormalCharacter PickGenderedNormalCharacter()
    {
        isFemale = Random.Range(0, 2) == 0;
        return PickGenderedNormalCharacter(isFemale);
    }

    /// <summary>
    /// Returns the matching NormalCharacter data for the given gender.
    /// Use this overload when the caller already decided gender and needs to reuse it.
    /// </summary>
    private NormalCharacter PickGenderedNormalCharacter(bool isFemale)
    {
        return isFemale ? characterSetup.normalFemale : characterSetup.normalMale;
    }

    /// <summary>
    /// Applies the base character sprite and one randomly chosen normal accessory
    /// (Mask or Ear). Used by both normal and anomaly character setups.
    /// </summary>
    private void ApplyNormalCharacter(NormalCharacter normalCharacter)
    {
        // Pick a random body sprite from the character's sprite pool
        CharacterSprite.sprite = normalCharacter.CharacterSprites.RandomKey();

        // Randomly assign either a mask or ear accessory (or nothing if None is rolled)
        E_WearingNormalAccessory randomAccessory = RandomUtil.RandomEnumValue<E_WearingNormalAccessory>();
        switch (randomAccessory)
        {
            case E_WearingNormalAccessory.Mask:
                // Only apply mask if no ear is already set
                if (AccesorySprite.sprite == null)
                    MaskSprite.sprite = normalCharacter.Masks.RandomValue();
                break;
            case E_WearingNormalAccessory.Ear:
                // Only apply ear if no mask is already set
                if (MaskSprite.sprite == null)
                    AccesorySprite.sprite = normalCharacter.Ears.RandomValue();
                break;
                // E_WearingNormalAccessory.None: no accessory applied, leave sprites null
        }
    }

    /// <summary>
    /// Picks a random anomaly accessory from the gendered AnomolyAccessory bag and applies it.
    /// The anomaly layer always wins — it clears the conflicting normal slot before applying.
    /// Ear/Ribbon/Tail use AccesorySprite or TailSprite and clear MaskSprite where needed.
    /// Mask uses MaskSprite and clears AccesorySprite.
    /// Flame uses its own renderer, no conflict.
    /// </summary>
    private void ApplyAnomolyAccessory(AnomolyAccessory accessory)
    {
        E_WearingAnomolyAccessory anomolyAccessory = RandomUtil.RandomEnumValue<E_WearingAnomolyAccessory>();
        switch (anomolyAccessory)
        {
            // Ear replaces AccesorySprite and clears any existing mask
            case E_WearingAnomolyAccessory.Ear:
                MaskSprite.sprite = null;
                AccesorySprite.sprite = accessory.Ears.RandomValue();
                AccesorySprite.sortingOrder = -1; // Ensure ears render above masks since they share the same slot   
                break;

            // Mask replaces MaskSprite and clears any existing ear/accessory
            case E_WearingAnomolyAccessory.Mask:
                AccesorySprite.sprite = null;
                MaskSprite.sprite = accessory.Masks.RandomValue();
                break;

            // Ribbon replaces AccesorySprite and clears any existing mask
            case E_WearingAnomolyAccessory.Ribbon:
                MaskSprite.sprite = null;
                AccesorySprite.sprite = accessory.Ribbons.RandomValue();
                break;

            // Flame is on its own renderer — no conflict with Mask or AccesorySprite
            case E_WearingAnomolyAccessory.Flame:
                FlameSprite.sprite = accessory.Flames.RandomValue();
                break;

            // Tail is on its own renderer — no conflict with Mask or AccesorySprite
            case E_WearingAnomolyAccessory.Tail:
                TailSprite.sprite = accessory.Tails.RandomValue();
                break;
        }
    }

    // --- Enums ---

    private enum E_WearingNormalAccessory
    {
        Mask,
        Ear,
    }

    private enum E_WearingAnomolyAccessory
    {
        Ear,
        Mask,
        Ribbon,
        Flame,
        Tail
    }
}