using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelEditorSO", menuName = "LevelEditorScriptableObject", order = 1)]
public class LevelEditorSO : ScriptableObject
{
    public string[] frameType;
    public string[] sceneFolderName;
}
