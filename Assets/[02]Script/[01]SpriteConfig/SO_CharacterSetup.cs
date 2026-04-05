using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "SO_CharacterSetup", menuName = "Scriptable Objects/New Character SO/SO_CharacterSetup")]
public class SO_CharacterSetup : ScriptableObject
{
    [Header("Normal Character")]
    public NormalCharacter normalFemale;
    public NormalCharacter normalMale;
    [Space(10)]
    [Header("Anomoly Character")]
    public List<Sprite> AnomolySprite;
    public AnomolyAccessory anomolyFemaleAccessory;
    public AnomolyAccessory anomolyMaleAccessory;

}

[System.Serializable]
public struct NormalCharacter {
    [SerializedDictionary("Normal", "Wet")]
    public SerializedDictionary<Sprite, Sprite> CharacterSprites;
    public List<Sprite> Masks;
    public List<Sprite> Ears;
}

[System.Serializable]
public struct AnomolyAccessory {
    public List<Sprite> Masks;
    public List<Sprite> Ears;
    public List<Sprite> Ribbons;
    public List<Sprite> Tails;
    public List<Sprite> Flames;
}

