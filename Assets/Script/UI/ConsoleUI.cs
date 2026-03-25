using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConsoleUI : MonoBehaviour
{
    public bool show;
    
    public TMP_Dropdown functionOption;
    public List<GameObject> detailPanels;
    
    [Header("范围Gizmos")]
    public GizmosControl gizmosControl;
    public List<Toggle> toggles;

    private void Start()
    {
        // 面板选择与显示监听
        functionOption.onValueChanged.AddListener((arg) => ChangeDetailPanel());

        // Gizmos面板
        for (var i = 0; i < toggles.Count; i++)
        {
            var toggle = toggles[i];
            var index = i;
            toggle.onValueChanged.AddListener(arg =>
            {
                gizmosControl.SetBool(index, arg); 
            });
        }
        
        
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


    
    public void OnConsole(InputValue value)
    {
        Debug.Log("Press");
        if (value.isPressed)
        {
            show = !show;
            gameObject.SetActive(show);
        }
    }
}