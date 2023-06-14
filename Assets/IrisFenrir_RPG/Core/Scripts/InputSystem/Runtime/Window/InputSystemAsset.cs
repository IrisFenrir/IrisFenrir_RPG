using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using IrisFenrir.InputSystem;

[CreateAssetMenu(fileName = "New Input System Asset", menuName = "IrisFenrir/InputSystemAsset")]
public class InputSystemAsset : ScriptableObject
{

    public List<KeyDataUnit> keys = new List<KeyDataUnit>() { };

    
}
