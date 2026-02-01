using AYellowpaper.SerializedCollections;
using System.Collections;   
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_FoxEar_F_Normal", menuName = "Scriptable Objects/SO_FoxEar_F_Normal")]
public class SO_FoxEar_F_Normal : ScriptableObject
{
    [SerializedDictionary("Normal", "Wet")]
    public SerializedDictionary<Sprite, Sprite> sprites;
}
