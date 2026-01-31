using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "SO_Order", menuName = "Scriptable Objects/Dialogue/SO_Order")]
public class SO_Order : ScriptableObject
{
    public List<string> orders;
}
