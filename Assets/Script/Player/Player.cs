using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    common,
    fire,
    electricity
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
    public Element element;

    public void AddEP(int num)
    {
        if (EP + num > MaxEP)   return;
        EP += num;
        UIManager.Instance.UpdateEnergyUI(EP);
    }   
    public void MinusEP(int num)
    {
        if (EP - num < 0)   return;
        EP -= num;
        UIManager.Instance.UpdateEnergyUI(EP);
    }

    public void AddHP(int num)
    {
        if (HP + num > MaxHP) return;
        HP += num;
        UIManager.Instance.UpdateHealthUI(HP);
    }
    public void MinusHP(int num)
    {
        if (HP - num <= 0)
        {
            Debug.Log("Player死亡");
        }
        HP -= num;
        UIManager.Instance.UpdateHealthUI(HP);
    }
    
    
    void Start()
    {
        HP = MaxHP;
        EP = MaxEP;
        UIManager.Instance.UpdateHealthUI(HP);
    }

    public void SetElement(Element e)
    {
        element = e;
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.M))
        {
            AddHP(15);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            MinusHP(15);
        }*/
    }
}
