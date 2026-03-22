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
    
    void Start()
    {
        HP = MaxHP;
        MaxHP = MaxEP;
        UIManager.Instance.UpdateEnergyUI(EP);
    }

    public void SetElement(Element e)
    {
        element = e;
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.Instance.UpdateEnergyUI(EP);
        }*/
    }
}
