using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TPLevelTool : MonoBehaviour
{
    [Serializable]
    public class RoomInfo
    {
        public string roomName;
        public Transform spawnPoint;
    }

    [Serializable]
    public class LevelInfo
    {
        public string levelName;
        public string sceneName;
        public List<RoomInfo> rooms;
    }

    [Header("关卡信息")]
    public List<LevelInfo> levels;

    private static TPLevelTool instance;
    public static TPLevelTool Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region 关键接口函数

    /// <summary>
    /// 传送到指定关卡的指定房间
    /// </summary>
    /// <param name="levelIndex">关卡索引</param>
    /// <param name="roomIndex">房间索引</param>
    public void TeleportToRoom(int levelIndex, int roomIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Count)
        {
            LevelInfo level = levels[levelIndex];
            if (roomIndex >= 0 && roomIndex < level.rooms.Count)
            {
                RoomInfo room = level.rooms[roomIndex];
                TeleportToRoom(level, room);
            }
            else
            {
                Debug.LogError($"房间索引 {roomIndex} 超出范围");
            }
        }
        else
        {
            Debug.LogError($"关卡索引 {levelIndex} 超出范围");
        }
    }

    /// <summary>
    /// 传送到指定关卡的指定房间
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    /// <param name="roomName">房间名称</param>
    public void TeleportToRoom(string levelName, string roomName)
    {
        LevelInfo level = levels.Find(l => l.levelName == levelName);
        if (level != null)
        {
            RoomInfo room = level.rooms.Find(r => r.roomName == roomName);
            if (room != null)
            {
                TeleportToRoom(level, room);
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

    #endregion

    private void TeleportToRoom(LevelInfo level, RoomInfo room)
    {
        if (room.spawnPoint == null)
        {
            Debug.LogError($"房间 {room.roomName} 的 spawnPoint 未设置");
            return;
        }

        // 检查当前场景是否与目标场景相同
        if (SceneManager.GetActiveScene().name == level.sceneName)
        {
            // 同场景内传送
            TeleportPlayer(room.spawnPoint.position, room.spawnPoint.rotation);
        }
        else
        {
            // 跨场景传送
            StartCoroutine(LoadSceneAndTeleport(level.sceneName, room.spawnPoint.position, room.spawnPoint.rotation));
        }
    }

    private void TeleportPlayer(Vector3 position, Quaternion rotation)
    {
        // 查找玩家
        var player = GameObject.FindGameObjectWithTag("Player");
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
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        // 等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // 场景加载完成后传送玩家
        TeleportPlayer(position, rotation);
    }

    #region 编辑器工具函数
    
    // 添加新关卡
    public void AddLevel()
    {
        LevelInfo newLevel = new LevelInfo
        {
            levelName = "新关卡",
            sceneName = SceneManager.GetActiveScene().name,
            rooms = new List<RoomInfo>()
        };
        levels.Add(newLevel);
    }
    
    // 为指定关卡添加新房间
    public void AddRoom(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Count)
        {
            RoomInfo newRoom = new RoomInfo
            {
                roomName = "新房间",
                spawnPoint = transform
            };
            levels[levelIndex].rooms.Add(newRoom);
        }
    }

    #endregion
}