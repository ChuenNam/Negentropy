using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConsoleUI : MonoBehaviour
{
    public bool show;
    
    public TMP_Dropdown functionOption;
    public List<GameObject> detailPanels;
    
    [Header("玩家信息")]
    public PlayerInfoTool playerInfoTool;
    public List<Toggle> toggles3;
    public List<TMP_InputField> inputFields;  
    
    [Header("范围Gizmos")]
    public GizmosControl gizmosControl;
    public List<Toggle> toggles1;
    
    [Header("创建敌人")]
    public GenerateTool generateTool;
    public Button generateButton;
    public List<Toggle> toggles2;

    private void Start()
    {
        // 面板选择与显示监听
        functionOption.onValueChanged.AddListener((arg) => ChangeDetailPanel());

        #region Gizmos面板
        for (var i = 0; i < toggles1.Count; i++)
        {
            var toggle = toggles1[i];
            var index = i;
            toggle.onValueChanged.AddListener(arg =>
            {
                gizmosControl.SetBool(index, arg); 
            });
        }
        #endregion

        #region 创建敌人面板
        for (var i = 0; i < toggles2.Count; i++)
        {
            var toggle = toggles2[i];
            var index = i;
            toggle.onValueChanged.AddListener(arg =>
            {
                generateTool.SetBool(index, arg);
            });
        }
        generateButton.onClick.AddListener(generateTool.GenerateEnemy);
        #endregion

        #region 玩家信息面板
        for (var i = 0; i < toggles3.Count; i++)
        {
            var toggle = toggles3[i];
            var index = i;
            toggle.onValueChanged.AddListener(arg =>
            {
                playerInfoTool.SetBool(index, arg);
            });
            toggle.isOn = playerInfoTool.GetBool(index);
        }

        for (var i = 0; i < inputFields.Count; i++)
        {
            var inputField = inputFields[i];
            var index = i;
            inputField.onValueChanged.AddListener(arg =>
            {
                playerInfoTool.SetInt(index, int.Parse(arg));
            });
            inputField.text = playerInfoTool.GetInt(index).ToString();
        }
        #endregion
        
    }

    private void ChangeDetailPanel()
    {
        var value = functionOption.value;
        // 隐藏所有面板
        foreach (var panel in detailPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
        // 显示对应的面板
        if (value >= 0 && value < detailPanels.Count && detailPanels[value] != null)
        {
            detailPanels[value].SetActive(true);
        }
    }
    
    public void ShowConsole()
    {
        show = !show;
        gameObject.SetActive(show);
    }
}