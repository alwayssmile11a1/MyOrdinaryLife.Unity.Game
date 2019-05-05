using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EpisodeScriptableObject
{
    public  string                      episodeName;
    public  int                         numberOfScene; 
}

[CreateAssetMenu(fileName = "SceneNumberSO", menuName = "SceneNumberScriptableObject", order = 1)]
public class SceneAmountScriptableObject : ScriptableObject
{
    public  EpisodeScriptableObject[]   episodeScriptableObjects;
}
