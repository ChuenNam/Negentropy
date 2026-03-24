using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class BaseObject : MonoBehaviour
{
    public bool canInteract = true;
    public float range = 5;
    [Header("能量条配置")]
    public GameObject energyBarPrefab;  // 能量条预制体
    public Transform energyBarFollowPoint;  // 能量条跟随的点
    [Header("能量配置")]
    public int maxEnergy = 100;
    public int currentEnergy;
    private bool showBar;  // 显示能量条
    protected GameObject energyBarInstance; // 能量条实例
    private Image energyFillImage;  // 能量条填充Image
    
    protected Transform player;
    public Action OnEnergyFill;
    public Action OnEnergyEmpty;
    public Action OnEnergyChange;

    protected virtual void OnEnable()
    {
        currentEnergy = maxEnergy;  // 初始化能量
        CreateEnergyBar();  // 创建能量条实例（和敌人同时出现）
        player = GameObject.Find("Player").transform;

        OnEnergyEmpty += () => Debug.Log("能量为0");
    }

    protected virtual void OnDisable()
    {
        OnEnergyEmpty = null;
        OnEnergyFill = null;
        OnEnergyChange = null;
    }

    // 创建能量条并绑定跟随逻辑
    private void CreateEnergyBar()
    {
        if (energyBarPrefab == null) return;
        
        // 实例化能量条（父物体设为Canvas）
        energyBarInstance = Instantiate(energyBarPrefab, GameObject.Find("Canvas").transform);
        energyBarInstance.SetActive(true);
        
        // 获取能量条填充组件
        energyFillImage = energyBarInstance.transform.GetChild(0).GetComponent<Image>();
        
        // 初始化能量条显示
        UpdateEnergyBar();
    }

    private void Update()
    {
        // 实时更新能量条位置
        if (energyBarInstance != null && energyBarFollowPoint != null)
        {
            var dis = Vector3.Distance(player.position, energyBarFollowPoint.position);
            if (dis <= range)
            {
                // 判断敌人是否在相机视野内
                var viewportPos = Camera.main.WorldToViewportPoint(energyBarFollowPoint.position);
                var isInView = viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1 && viewportPos.z > 0;

                if (isInView)
                {
                    // 更新位置
                    UpdateEnergyBarPosition();
                }
                energyBarInstance.SetActive(isInView);
            }
            else
            {
                energyBarInstance.SetActive(false);
            }
        }
        
        if (!canInteract)   
            energyBarInstance.SetActive(false);

        var atk = player.GetComponent<Attack>();
        if (energyBarInstance.activeInHierarchy)
        {
            if (atk.enemies.Contains(this)) return;
            atk.enemies.Add(this);
        }
        else
        {
            if (atk.enemies.Contains(this))
            {
                atk.enemies.Remove(this);
            }
        }
        
        // 测试：按Z键扣能量，按X键回能量
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(20);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Heal(20);
        }
    }

    // 更新能量条位置（3D坐标转屏幕UI坐标）
    private void UpdateEnergyBarPosition()
    {
        // 敌人世界坐标
        Vector3 worldPos = energyBarFollowPoint.position;
        
        // 世界坐标转屏幕UI坐标
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        
        // 设置能量条UI的位置
        RectTransform rectTransform = energyBarInstance.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent.GetComponent<RectTransform>(),
            screenPos,
            null,
            out Vector2 localPos
        );
        rectTransform.localPosition = localPos;
    }


    // 更新能量条填充量
    public void UpdateEnergyBar()
    {
        if (energyFillImage == null || maxEnergy == -1) return;
        float fillAmount = (float)currentEnergy / maxEnergy;
        energyFillImage.fillAmount = Mathf.Clamp01(fillAmount);
    }

    public void ShowBar(bool state)
    {
        energyFillImage.gameObject.SetActive(state);
    }
    
    // 扣能量方法（外部调用）
    public void TakeDamage(int damage)
    {
        OnEnergyChange?.Invoke();
        if (maxEnergy == -1) return;

        currentEnergy = Mathf.Max(0, currentEnergy - damage);
        UpdateEnergyBar();
        
        // 能量为0时调用事件
        if (currentEnergy == 0)
        {
            OnEnergyEmpty?.Invoke();
        }
    }

    // 回能量方法（外部调用）
    public void Heal(int healAmount)
    {
        OnEnergyChange?.Invoke();
        if (maxEnergy == -1) return;
        
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + healAmount);
        UpdateEnergyBar();

        if (currentEnergy == maxEnergy)
        {
            OnEnergyFill?.Invoke();
        }
    }
    
    // 销毁能量条
    public void DestroyEnergyBar()
    {
        if (energyBarInstance != null)
        {
            Destroy(energyBarInstance);
        }

        if (player == null) return;
        var p = player.GetComponent<Attack>();
        p.target = null;
        p.enemies.Remove(this);
    }
    // 隐藏能量条
    public void HideEnergyBar()
    {
        canInteract = false;
        energyBarInstance.SetActive(false);
        
        if (player == null) return;
        var p = player.GetComponent<Attack>();
        p.target = null;
        p.enemies.Remove(this);
    }
    
    // 敌人销毁时同步销毁能量条
    private void OnDestroy()
    {
        DestroyEnergyBar();
    }

}