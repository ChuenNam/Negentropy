using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LootBox : MonoBehaviour
{
    public bool isLock = true;
    private bool canOpen = false;
    public UnityEvent unlockAction;
    public void Unlock() => isLock = false;
    
    
    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = new Color(1, .5f, 0, 1);
    }

    private void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.F))
            Open();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLock || !other.CompareTag("Player")) return;
        UIManager.Instance.tipsUI.OpenInteractTips();
        canOpen = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        UIManager.Instance.tipsUI.CloseInteractTips();
        canOpen = false;
    }

    private void Open()
    {
        isLock = false;
        unlockAction?.Invoke();
        unlockAction = null;
        UIManager.Instance.tipsUI.CloseInteractTips();
        Destroy(gameObject);
    }


    public void DebugXXX(string txt) => UIManager.Instance.tipsUI.OpenGetTips(txt);
    public void UnlockXXX()
    {
        UIManager.Instance.tipsUI.OpenGetTips("获得XXX");
    }
    
    public void UnlockSpike()
    {
        Player.Instance.GetComponent<FollowerBehavier>().lockSpike = false;
        UIManager.Instance.consoleUI.playerInfoTool.UpdatePlayerInfo();
        UIManager.Instance.tipsUI.OpenGetTips("解锁尖刺攻击(E)");
    }
    public void UnlockBall()
    {
        Player.Instance.GetComponent<FollowerBehavier>().lockBall = false;
        UIManager.Instance.consoleUI.playerInfoTool.UpdatePlayerInfo();
        UIManager.Instance.tipsUI.OpenGetTips("解锁重锤攻击(Q)");
    }
    public void UnlockFire()
    {
        Player.Instance.GetComponent<Attack>().LockFire(false);
        UIManager.Instance.consoleUI.playerInfoTool.UpdatePlayerInfo();
        UIManager.Instance.tipsUI.OpenGetTips("解锁火元素获取与效果");
    }
    public void UnlockElectricity()
    {
        Player.Instance.GetComponent<Attack>().lockElectricity = false;
        UIManager.Instance.consoleUI.playerInfoTool.UpdatePlayerInfo();
        UIManager.Instance.tipsUI.OpenGetTips("解锁雷元素效果");
    }
    public void UnlockBoom()
    {
        Player.Instance.GetComponent<Attack>().lockBoom = false;
        UIManager.Instance.consoleUI.playerInfoTool.UpdatePlayerInfo();
        UIManager.Instance.tipsUI.OpenGetTips("解锁爆炸效果");
    }

    public void MaxEP_Up()
    {
        Player.Instance.MaxEP++;
        Player.Instance.EP++;
        UIManager.Instance.consoleUI.playerInfoTool.UpdatePlayerInfo();
        UIManager.Instance.playerUI.UpdateEnergyUI(Player.Instance.EP);
        UIManager.Instance.tipsUI.OpenGetTips("能量上限+1");
    }
    public void MaxHP_Up(int value)
    {
        Player.Instance.MaxHP += value;
        Player.Instance.HP += value;
        UIManager.Instance.consoleUI.playerInfoTool.UpdatePlayerInfo();
        UIManager.Instance.playerUI.UpdateHealthUI(Player.Instance.HP);
        UIManager.Instance.tipsUI.OpenGetTips($"生命上限+{value}");
    }
}
