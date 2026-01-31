using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_FemaleBody_Anomaly", menuName = "Scriptable Objects/SO_FemaleBody_Anomaly")]
public class SO_FemaleBody_Anomaly : ScriptableObject
{
    [SerializedDictionary("Normal", "Wet")]
    public SerializedDictionary<Sprite, Sprite> sprites;
}
