using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoTool : MonoBehaviour
{
    private Player player;
    public bool isInvincible;
    public int maxHP;
    public int maxEP;
    public float playerSpeed;
    public bool lockFire;
    public bool lockElectric;
    public bool lockSpike;
    public bool lockBall;

    private void Update()
    { 
        player.gameObject.tag = isInvincible ? "Default" : "Player";
        
    }


    public void Start()
    {
        player = Player.Instance;
    }
}
