using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private void Awake()   
    {
        if (Instance == null)  
            Instance = this;
        else   
            Destroy(gameObject);
    }

    
    public PlayerUI playerUI;
    public ConsoleUI consoleUI;
    public LogUI logUI;
}
