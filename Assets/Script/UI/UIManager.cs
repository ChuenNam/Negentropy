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

    public List<Image> energyImgs;

    public void UpdateEnergyUI(int num)
    {
        for (var i = 0; i < num; i++)
        {
            energyImgs[i].color = Color.green;
        }
        for (var i = num; i < energyImgs.Count; i++)
        {
            energyImgs[i].color = Color.white;
        }
    }
}
