using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SO_Mask_Anamoly", menuName = "Scriptable Objects/SO_Mask_Anamoly")]
public class SO_Mask_Anamoly : ScriptableObject
{
    public List<Sprite> Mask_Anamoly_Normal;
    public List<Sprite> Mask_Anamoly_Wet;
}
