using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SO_Getout", menuName = "Scriptable Objects/Dialogue/SO_Getout")]
public class SO_Getout : ScriptableObject
{
    public List<string> dialogue;
}
