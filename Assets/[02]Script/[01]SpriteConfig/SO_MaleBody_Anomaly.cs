using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_MaleBody_Anomaly", menuName = "Scriptable Objects/SO_MaleBody_Anomaly")]
public class SO_MaleBody_Anomaly : ScriptableObject
{
    [SerializedDictionary("Normal", "Wet")]
    public SerializedDictionary<Sprite, Sprite> MaleBody_Anomaly;
}
