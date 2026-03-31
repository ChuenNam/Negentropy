using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Negentropy/LevelConfig", order = 2)]
public class LevelConfig : ScriptableObject
{
    public List<LevelData> levels;
}