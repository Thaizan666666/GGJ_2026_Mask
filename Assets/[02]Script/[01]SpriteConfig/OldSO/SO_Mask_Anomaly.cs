using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Mask_Anomaly", menuName = "Scriptable Objects/SO_Mask_Anomaly")]
public class SO_Mask_Anomaly : ScriptableObject
{
    [SerializedDictionary("Normal", "Wet")]
    public SerializedDictionary<Sprite, Sprite> sprites;

}
