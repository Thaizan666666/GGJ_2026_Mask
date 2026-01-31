
using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_FoxEar_M_Normal", menuName = "Scriptable Objects/SO_FoxEar_M_Normal")]
public class SO_FoxEar_M_Normal : ScriptableObject
{
    [SerializedDictionary("Normal","Wet")]
    public SerializedDictionary<Sprite, Sprite> sprites;
}
