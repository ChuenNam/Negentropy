// 编辑器工具类，用于在编辑器中操作
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public static class LevelConfigEditor
{
    [MenuItem("Negentropy/Create Level Data")]
    public static void CreateLevelData()
    {
        // 创建新的LevelData ScriptableObject
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        levelData.rooms = new List<RoomInfo>();
        
        // 确保目录存在
        string directory = "Assets/Resources/Config/Levels";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        // 保存到Resources目录
        AssetDatabase.CreateAsset(levelData, $"{directory}/NewLevelData.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("LevelData创建成功");
    }
    
    [MenuItem("Negentropy/Create Level Config")]
    public static void CreateLevelConfig()
    {
        // 创建新的LevelConfig ScriptableObject
        LevelConfig config = ScriptableObject.CreateInstance<LevelConfig>();
        config.levels = new List<LevelData>();
        
        // 确保目录存在
        string directory = "Assets/Resources/Config";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        // 保存到Resources目录
        AssetDatabase.CreateAsset(config, $"{directory}/LevelConfig.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("LevelConfig创建成功");
    }
}
#endif