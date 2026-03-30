using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_FoxEar_Anomaly", menuName = "Scriptable Objects/SO_FoxEar_Anomaly")]
public class SO_FoxEar_Anomaly : ScriptableObject
{
    [SerializedDictionary("Normal", "Wet")]
    public SerializedDictionary<Sprite, Sprite> sprites;
}
