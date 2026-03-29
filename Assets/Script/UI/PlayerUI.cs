using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public RectTransform healthBar;
    public RectTransform energyInitRect;
    
    public List<Image> energyImgs;
    public List<Image> elementImgs;

    /*private void Start()
    {
        UpdateEnergyUI(Player.Instance.EP);
        UpdateHealthUI(Player.Instance.HP);
    }*/

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UpdateEnergyUI(Player.Instance.EP);
            UpdateHealthUI(Player.Instance.HP);
        }
    }


    public void AddEnergyUI()
    {
        var EP_prefab = Resources.Load<GameObject>($"Prefab/UI/EP");
        var ep= Instantiate(EP_prefab, energyInitRect);
        energyImgs.Add(ep.GetComponent<Image>());
    }
    public void UpdateMaxHealthUI(bool heal = false)
    {
        var maxHp = Player.Instance.MaxHP;
        if (maxHp < Player.Instance.HP)
            Player.Instance.HP = maxHp;

        var hp = heal ? maxHp : Player.Instance.HP;
        var rect = transform.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(maxHp * 5 + 5, rect.sizeDelta.y);    // 血条框
        healthBar.sizeDelta = new Vector2(hp, healthBar.sizeDelta.y);   // 血条
    }
    
    
    public void UpdateEnergyUI(int num)
    {
        if (Player.Instance.MaxEP != energyImgs.Count)
        {
            var sub = Player.Instance.MaxEP - energyImgs.Count;
            if (sub < 0)
            {
                Player.Instance.EP = Player.Instance.MaxEP;
                num = Player.Instance.EP;
                for (var i = 0; i < -sub; i++)
                {
                    Destroy(energyImgs[i].gameObject);
                    energyImgs.Remove(energyImgs[i]);
                }
            }
            else if (sub > 0)
            {
                for (var i = 0; i < sub; i++)
                {
                    AddEnergyUI();
                }
            }
        }
        
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
        var rect = transform.GetComponent<RectTransform>();
        if (Player.Instance.MaxHP * 5 + 5 != (int)rect.sizeDelta.x)
        {
            UpdateMaxHealthUI();
            return;
        }
        healthBar.sizeDelta = new Vector2(num, healthBar.sizeDelta.y);
    }
}
