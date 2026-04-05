using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SO_Greeting", menuName = "Scriptable Objects/Dialogue/SO_Greeting")]
public class SO_Greeting : ScriptableObject
{
    public List<string> dialogue;
}
