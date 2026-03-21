using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敌人控制器（世界坐标3D血条实现）
/// </summary>
public class EnemyController : MonoBehaviour
{
    [Header("血条配置")]
    public GameObject healthBarPrefab;    // 3D血条预制体
    public Transform healthBarFollowPoint; // 敌人头顶跟随点
    public bool faceCamera = true;        // 血条始终面向相机

    [Header("血量配置")]
    public float maxHealth = 100;
    private float currentHealth;
    private GameObject healthBarInstance;
    private Image healthFillImage;

    public Transform player;

    private void Awake()
    {
        currentHealth = maxHealth;
        CreateHealthBar();
    }

    private void OnEnable()
    {
        player = GameObject.Find("Player").transform;
    }
    
    /// <summary>
    /// 世界坐标方式创建血条
    /// </summary>
    private void CreateHealthBar()
    {
        if (healthBarPrefab == null) return;

        // 直接在世界空间实例化，父物体是Canvas
        healthBarInstance = Instantiate(healthBarPrefab, transform.GetChild(0));
        
        // 强制放在跟随点的世界坐标位置
        healthBarInstance.transform.position = healthBarFollowPoint.position;
        healthBarInstance.transform.rotation = Quaternion.identity;

        // 获取血条图片
        healthFillImage = healthBarInstance.GetComponentInChildren<Image>();
        
        UpdateHealthBar();
    }

    private void Update()
    {
        if (healthBarInstance != null && healthBarFollowPoint != null)
        {
            UpdateHealthBarPosition_World();
        }

        // 测试按键
        if (Input.GetKeyDown(KeyCode.Z)) TakeDamage(20);
        if (Input.GetKeyDown(KeyCode.X)) Heal(20);
    }

    /// <summary>
    /// 世界坐标更新血条位置（核心！）
    /// </summary>
    private void UpdateHealthBarPosition_World()
    {
        // 直接赋值世界坐标，不做任何UI转换
        healthBarInstance.transform.position = healthBarFollowPoint.position;

        // 可选：血条永远面向主相机
        if (faceCamera)
        {
            healthBarInstance.transform.rotation = Camera.main.transform.rotation;
            //healthBarInstance.transform.LookAt(healthBarInstance.transform.position + Camera.main.transform.forward);
        }

        // 判断是否在视野内
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(healthBarFollowPoint.position);
        bool isInView = viewportPos.x > 0 && viewportPos.x < 1
                     && viewportPos.y > 0 && viewportPos.y < 1
                     && viewportPos.z > 0;

        healthBarInstance.SetActive(isInView);
    }

    /// <summary>
    /// 更新血条显示
    /// </summary>
    private void UpdateHealthBar()
    {
        if (healthFillImage == null) return;
        healthFillImage.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Destroy(healthBarInstance);
            Destroy(gameObject);
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
        UpdateHealthBar();
    }

    private void OnDestroy()
    {
        if (healthBarInstance != null) Destroy(healthBarInstance);
    }
}