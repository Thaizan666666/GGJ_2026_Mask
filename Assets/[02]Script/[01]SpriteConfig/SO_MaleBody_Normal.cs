using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_MaleBody_Normal", menuName = "Scriptable Objects/SO_MaleBody_Normal")]
public class SO_MaleBody_Normal : ScriptableObject
{
    [SerializedDictionary("Normal", "Wet")]
    public SerializedDictionary<Sprite, Sprite> sprites;

}
