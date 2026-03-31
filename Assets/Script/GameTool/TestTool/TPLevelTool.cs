using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class RoomInfo
{
    public string roomName;
    public Vector3 position;
    public Quaternion rotation;
}


public class TPLevelTool : MonoBehaviour
{
    [Header("配置文件")]
    public LevelConfig levelConfig;

    private static TPLevelTool instance;
    public static TPLevelTool Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeConfig();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeConfig()
    {
        if (levelConfig == null)
        {
            Debug.LogError("请在Inspector面板中添加LevelConfig ScriptableObject");
        }
    }

    #region 关键接口函数
    
    // 传送到指定关卡的指定房间
    public void TeleportToRoom(int levelIndex, int roomIndex)
    {
        if (levelConfig == null)
        {
            Debug.LogError("配置文件未初始化");
            return;
        }
        // 遍历所有配置文件查找关卡
        LevelData targetLevel = null;
        if (levelIndex <= levelConfig.levels.Count)
        {
            targetLevel = levelConfig.levels[levelIndex];
        }
        if (targetLevel != null)
        {
            if (roomIndex >= 0 && roomIndex < targetLevel.rooms.Count)
            {
                var room = targetLevel.rooms[roomIndex];
                TeleportToRoom(targetLevel, room);
            }
            else Debug.LogError($"房间索引 {roomIndex} 超出范围");
        }
        else Debug.LogError($"关卡索引 {levelIndex} 超出范围");
    }
    
    // 传送到指定关卡的指定房间
    public void TeleportToRoom(string levelName, string roomName)
    {
        if (levelConfig == null)
        {
            Debug.LogError("配置文件未初始化");
            return;
        }
        // 遍历所有配置文件查找关卡
        LevelData targetLevel = null;
        if (levelConfig.levels is { Count: > 0 })
        {
            targetLevel = levelConfig.levels.Find(l => l.levelName == levelName);
        }
        if (targetLevel != null)
        {
            var room = targetLevel.rooms.Find(r => r.roomName == roomName);
            if (room != null)
                TeleportToRoom(targetLevel, room);
            
            else Debug.LogError($"未找到房间: {roomName}");
        }
        else Debug.LogError($"未找到关卡: {levelName}");
    }

    #endregion

    private void TeleportToRoom(LevelData level, RoomInfo room)
    {
        // 检查当前场景是否与目标场景相同
        if (SceneManager.GetActiveScene().name == level.sceneName)
        {
            // 同场景内传送
            TeleportPlayer(room.position, room.rotation);
        }
        else
        {
            // 跨场景传送
            StartCoroutine(LoadSceneAndTeleport(level.sceneName, room.position, room.rotation));
        }
    }

    private void TeleportPlayer(Vector3 position, Quaternion rotation)
    {
        // 查找玩家
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = position;
            player.transform.rotation = rotation;
            Debug.Log($"已传送到: {position}");
        }
        else
        {
            Debug.LogError("未找到玩家");
        }
    }

    private IEnumerator LoadSceneAndTeleport(string sceneName, Vector3 position, Quaternion rotation)
    {
        // 加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        // 等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // 场景加载完成后传送玩家
        TeleportPlayer(position, rotation);
    }

    /*#region 编辑器工具函数

    /// <summary>
    /// 添加新关卡
    /// </summary>
    public void AddLevel(LevelData levelData)
    {
        if (levelConfig == null)
        {
            Debug.LogError("配置文件未初始化");
            return;
        }
        
        // 添加到第一个配置文件中
        LevelConfig config = levelConfig;
        if (config.levels == null)
        {
            config.levels = new List<LevelData>();
        }
        
        config.levels.Add(levelData);
        
        // 标记为已修改，以便Unity保存
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        Debug.Log("关卡添加成功");
    }

    /// <summary>
    /// 为指定关卡添加新房间
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    /// <param name="roomName">房间名称</param>
    /// <param name="position">房间位置</param>
    /// <param name="rotation">房间旋转</param>
    public void AddRoom(string levelName, string roomName, Vector3 position, Quaternion rotation)
    {
        if (levelConfig == null)
        {
            Debug.LogError("配置文件未初始化");
            return;
        }
        
        // 遍历所有配置文件查找关卡
        LevelData targetLevel = null;
        LevelConfig targetConfig = null;
        foreach (var config in levelConfig)
        {
            if (config != null && config.levels != null)
            {
                targetLevel = config.levels.Find(l => l.levelName == levelName);
                if (targetLevel != null)
                {
                    targetConfig = config;
                    break;
                }
            }
        }
        
        if (targetLevel != null && targetConfig != null)
        {
            RoomInfo newRoom = new RoomInfo
            {
                roomName = roomName,
                position = position,
                rotation = rotation
            };
            targetLevel.rooms.Add(newRoom);
            
            // 标记为已修改，以便Unity保存
            EditorUtility.SetDirty(targetLevel);
            EditorUtility.SetDirty(targetConfig);
            AssetDatabase.SaveAssets();
            Debug.Log("房间添加成功");
        }
        else
        {
            Debug.LogError($"未找到关卡: {levelName}");
        }
    }

    /// <summary>
    /// 更新房间信息
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    /// <param name="roomName">房间名称</param>
    /// <param name="position">新位置</param>
    /// <param name="rotation">新旋转</param>
    public void UpdateRoom(string levelName, string roomName, Vector3 position, Quaternion rotation)
    {
        if (levelConfig == null)
        {
            Debug.LogError("配置文件未初始化");
            return;
        }
        
        // 遍历所有配置文件查找关卡
        LevelData targetLevel = null;
        LevelConfig targetConfig = null;
        foreach (var config in levelConfig)
        {
            if (config != null && config.levels != null)
            {
                targetLevel = config.levels.Find(l => l.levelName == levelName);
                if (targetLevel != null)
                {
                    targetConfig = config;
                    break;
                }
            }
        }
        
        if (targetLevel != null && targetConfig != null)
        {
            RoomInfo room = targetLevel.rooms.Find(r => r.roomName == roomName);
            if (room != null)
            {
                room.position = position;
                room.rotation = rotation;
                
                // 标记为已修改，以便Unity保存
                EditorUtility.SetDirty(targetLevel);
                EditorUtility.SetDirty(targetConfig);
                AssetDatabase.SaveAssets();
                Debug.Log("房间更新成功");
            }
            else
            {
                Debug.LogError($"未找到房间: {roomName}");
            }
        }
        else
        {
            Debug.LogError($"未找到关卡: {levelName}");
        }
    }

    /// <summary>
    /// 获取所有关卡信息
    /// </summary>
    /// <returns>关卡信息列表</returns>
    public List<LevelData> GetAllLevels()
    {
        List<LevelData> allLevels = new List<LevelData>();
        
        if (levelConfig == null)
        {
            Debug.LogError("配置文件未初始化");
            return allLevels;
        }
        
        foreach (var config in levelConfig)
        {
            if (config != null && config.levels != null)
            {
                allLevels.AddRange(config.levels);
            }
        }
        
        return allLevels;
    }

    #endregion*/
}

