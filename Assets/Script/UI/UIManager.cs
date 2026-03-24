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
    public List<Image> elementImgs;
    public RectTransform healthBar;

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

    public void UpdateElementUI(Element element)
    {
        element.SetElementColor(elementImgs[1]);
    }
    public void UpdateElementUI(int num, Element element)
    {
        elementImgs[0].color = num == 1 ? Color.cyan : Color.white;
        element.SetElementColor(elementImgs[1]);
    }

    public void UpdateHealthUI(int num)
    {
        healthBar.sizeDelta = new Vector2(num, healthBar.sizeDelta.y);
    }
}
