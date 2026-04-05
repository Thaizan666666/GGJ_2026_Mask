using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;   

[CreateAssetMenu(fileName = "SO_Order_Normal", menuName = "Scriptable Objects/Dialogue/SO_Order_Normal")]
public class SO_Order_Normal : ScriptableObject
{
    public List<string> orders;
}
