using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoTool : MonoBehaviour
{
    private Player player;
    private Move move;
    private Attack attack;
    private FollowerBehavier followerBehavior;
    private Action OnValueChange;
    
    public bool isInvincible;
    public int maxHP;
    public int maxEP;
    public float speed;
    public bool lockSpike;
    public bool lockBall;
    public bool lockFire;
    public bool lockElectric;
    public bool lockBoom;

    public void Start()
    {
        player = Player.Instance;
        move = player.GetComponent<Move>();
        attack = player.GetComponent<Attack>();
        followerBehavior = player.GetComponent<FollowerBehavier>();
        
        // 初始化
        UpdatePlayerInfo();
        
        // 添加修改监听
        OnValueChange += () =>
        {
            player.gameObject.tag = isInvincible ? "Untagged" : "Player";
            
            player.MaxHP = maxHP;
            player.MaxEP = maxEP;
            move.maxSpeed = speed;
            
            followerBehavior.lockSpike = lockSpike;
            followerBehavior.lockBall = lockBall;
            
            attack.lockFire = lockFire;
            attack.lockElectricity = lockElectric;
            attack.lockBoom = lockBoom;
        };
    }

    public void UpdatePlayerInfo()
    {
        maxHP = player.MaxHP;
        maxEP = player.MaxEP;
        speed = move.maxSpeed;
        lockSpike = followerBehavior.lockSpike;
        lockBall = followerBehavior.lockBall;
        lockFire = attack.lockFire;
        lockElectric = attack.lockElectricity;
        lockBoom = attack.lockBoom;
    }
    
    public bool GetBool(int index)
    {
        return index switch
        {
            0 => isInvincible,
            1 => lockSpike,
            2 => lockBall,
            3 => lockFire,
            4 => lockElectric,
            5 => lockBoom,
            _ => false
        };
    }
    public void SetBool(int index, bool value)
    {
        switch (index)
        {
            case 0: isInvincible = value; break;
            case 1: lockSpike = value; break;
            case 2: lockBall = value; break;
            case 3: lockFire = value; break;
            case 4: lockElectric = value; break;
            case 5: lockBoom = value; break;
        }
        OnValueChange?.Invoke();
    }
    public int GetInt(int index)
    {
        return index switch
        {
            0 => maxHP,
            1 => maxEP,
            2 => (int)speed,
            _ => 0
        };
    }
    public void SetInt(int index, int value)
    {
        switch (index)
        {
            case 0: maxHP = value; break;
            case 1: maxEP = value; break;
            case 2: speed = value; break;
        }
        OnValueChange?.Invoke();
    }
    
    private void OnDisable() => OnValueChange = null;
}
