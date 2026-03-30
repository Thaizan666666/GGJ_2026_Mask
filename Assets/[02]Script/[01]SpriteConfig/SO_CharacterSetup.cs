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
    public AnomolyCharacter anomoly;  

}

[System.Serializable]
public struct NormalCharacter {
    [SerializedDictionary("Normal", "Wet")]
    public SerializedDictionary<Sprite, Sprite> CharacterSprites;
    public List<Sprite> Masks;
    public List<Sprite> Ears;
}

[System.Serializable]
public struct AnomolyCharacter {
    public Sprite AnomolySprite;
    [Space]
    [Header("Female")]
    public Sprite EarFemaleSprite;
    public List<Sprite> MaskFemaleSprite;
    public Sprite RibbonFemaleSprite;
    public Sprite TailFemale;
    public Sprite FlameFemaleSprite;

    [Header("Male")]
    public Sprite EarMaleSprite;
    public List<Sprite> MaskMaleSprite;   
    public Sprite RibbonMaleSprite;
    public Sprite TailMale;
    public Sprite FlameMaleSprite;
}
