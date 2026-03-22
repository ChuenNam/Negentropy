using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public int health;
    public int killEnergy;
    public float range = 5;
    
    [Header("能量条配置")]
    public GameObject energyBarPrefab;  // 能量条预制体
    public Transform energyBarFollowPoint;  // 能量条跟随的点
    [Header("能量配置")]
    public float maxEnergy = 100;
    public float currentEnergy;
    private bool showBar;  // 显示能量条
    private GameObject energyBarInstance; // 能量条实例
    public Image energyFillImage;  // 能量条填充Image

    public Transform player;

    private void OnEnable()
    {
        currentEnergy = maxEnergy;  // 初始化能量
        CreateEnergyBar();  // 创建能量条实例（和敌人同时出现）

        player = GameObject.Find("Player").transform;
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

        if (energyBarInstance.activeInHierarchy)
        {
            var atk = player.GetComponent<Attack>();
            if (atk.enemies.Contains(this)) return;
            atk.enemies.Add(this);
        }
        else
        {
            var atk = player.GetComponent<Attack>();
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
    private void UpdateEnergyBar()
    {
        if (energyFillImage == null) return;
        float fillAmount = currentEnergy / maxEnergy;
        energyFillImage.fillAmount = Mathf.Clamp01(fillAmount);
    }

    public void ShowBar(bool state)
    {
        energyFillImage.gameObject.SetActive(state);
    }
    
    // 扣能量方法（外部调用）
    public void TakeDamage(float damage)
    {
        currentEnergy = Mathf.Max(0, currentEnergy - damage);
        UpdateEnergyBar();
        
        // 能量为0时销毁敌人和能量条（同步消失）
        if (currentEnergy <= 0)
        {
            Destroy(energyBarInstance);
            Destroy(gameObject);
        }
    }

    // 回能量方法（外部调用）
    public void Heal(float healAmount)
    {
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + healAmount);
        UpdateEnergyBar();
    }
    
    // 敌人销毁时同步销毁能量条
    private void OnDestroy()
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

}
