using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Element
{
    common = 0,
    fire = 1,
    electricity = 2
}


public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private void Awake()   
    {
        if (Instance == null)  
            Instance = this;
        else   
            Destroy(gameObject);
    }
    
    [Header("资源")]
    public int MaxHP;
    public int MaxEP;
    [Space]
    public int HP;
    public int EP;

    public int elementPoint;
    public Element element;

    public void AddEP(int num)
    {
        // 获取元素逻辑
        if (element == Element.common || elementPoint == 0)
        {
            var tmp = elementPoint;
            for (var i = 0; i < EP+num - MaxEP; i++)
            {
                tmp++;
                if (tmp != 2 || (int)element >= 1) continue;
                tmp = 0;
                element += 1;
            }
            elementPoint = tmp;
            UIManager.Instance.playerUI.UpdateElementUI(elementPoint, element);
        }
        
        EP = Mathf.Min(MaxEP, EP + num);
        UIManager.Instance.playerUI.UpdateEnergyUI(EP);
    }   
    public void MinusEP(int num)
    {
        EP = Mathf.Max(0, EP - num);
        UIManager.Instance.playerUI.UpdateEnergyUI(EP);
    }

    public void SetEP(int num)
    {
        EP = num;
        UIManager.Instance.playerUI.UpdateEnergyUI(EP);
    }

    public void AddHP(int num)
    {
        if (HP + num > MaxHP) return;
        HP += num;
        UIManager.Instance.playerUI.UpdateHealthUI(HP);
    }
    public void MinusHP(int num)
    {
        if (HP - num <= 0)
        {
            Debug.Log("Player死亡");
        }
        HP -= num;
        UIManager.Instance.playerUI.UpdateHealthUI(HP);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddEP(1);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            MinusEP(1);
        }
    }
    
    void Start()
    {
        HP = MaxHP;
        EP = MaxEP;
        UIManager.Instance.playerUI.UpdateHealthUI(HP);
        UIManager.Instance.playerUI.UpdateEnergyUI(EP);
    }
}
