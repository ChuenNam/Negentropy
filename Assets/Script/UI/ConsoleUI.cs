using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TMPro;
using UnityEngine.UI;

public class ConsoleUI : MonoBehaviour
{
    public bool show;
    
    public Dropdown functionOption;
    public List<GameObject> detailPanels;
    
    [Header("玩家信息")]
    public PlayerInfoTool playerInfoTool;
    public List<Toggle> toggles3;
    public List<InputField> inputFields;  
    
    [Header("范围Gizmos")]
    public GizmosControl gizmosControl;
    public List<Toggle> toggles1;
    
    [Header("创建敌人")]
    public GenerateTool generateTool;
    public Button generateButton;
    public List<Toggle> toggles2;
    
    [Header("传送控制")]
    private TPLevelTool tpLevelTool;
    public Dropdown levelChoseDropdown;
    public Dropdown roomChoseDropdown;
    public Button TPButton;

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

        #region 传送控制
        
        tpLevelTool = TPLevelTool.Instance;
        var levels = tpLevelTool.levelConfig.levels;
        levelChoseDropdown.ClearOptions();
        // 添加关卡选项
        foreach (var level in levels)
        {
            levelChoseDropdown.options.Add(new Dropdown.OptionData(level.levelName));
        }
        levelChoseDropdown.RefreshShownValue();
        // 切换关卡时更新房间数据
        levelChoseDropdown.onValueChanged.AddListener(arg =>
        {
            roomChoseDropdown.ClearOptions();
            var level = levels[arg];
            foreach (var room in level.rooms)
            {
                roomChoseDropdown.options.Add(new Dropdown.OptionData(room.roomName));
            }
            roomChoseDropdown.RefreshShownValue();
        });
        levelChoseDropdown.onValueChanged?.Invoke(0);
        
        // 点击传送绑定
        TPButton.onClick.AddListener(() =>
        {
            var levelText = levelChoseDropdown.options[levelChoseDropdown.value].text;
            var roomText = roomChoseDropdown.options[roomChoseDropdown.value].text;
            tpLevelTool.TeleportToRoom(levelText, roomText);
        });
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