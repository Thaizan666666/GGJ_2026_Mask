using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_FemaleBody_Normal", menuName = "Scriptable Objects/SO_FemaleBody_Normal")]
public class SO_FemaleBody_Normal : ScriptableObject
{
    [SerializedDictionary("Normal", "Wet")]
    public SerializedDictionary<Sprite, Sprite> sprites;
}
