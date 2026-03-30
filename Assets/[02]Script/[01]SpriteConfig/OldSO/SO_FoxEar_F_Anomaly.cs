using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_FoxEar_F_Anomaly", menuName = "Scriptable Objects/SO_FoxEar_F_Anomaly")]
public class SO_FoxEar_F_Anomaly : ScriptableObject
{
    [SerializedDictionary("Normal", "Wet")]
    public SerializedDictionary<Sprite, Sprite> sprites;
}
