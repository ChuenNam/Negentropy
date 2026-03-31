using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "LevelData", menuName = "Negentropy/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public string sceneName;
    public List<RoomInfo> rooms;
}