using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Tail_Anomaly", menuName = "Scriptable Objects/SO_Tail_Anomaly")]
public class SO_Tail_Anomaly : ScriptableObject
{
    [SerializedDictionary("Normal", "Wet")]
    public SerializedDictionary<Sprite, Sprite> Tail_Anomaly;
}
