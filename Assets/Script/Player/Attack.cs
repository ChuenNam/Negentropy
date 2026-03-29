using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Attack : MonoBehaviour, IAttack
{
    [Header("攻击参数")]
    public float velocity;
    
    [SerializeField] private int damage = 10;
    public int Damage
    {
        get => damage; 
        set => damage = value;
    }
    public Element ElementType
    { 
        get => Player.Instance.element;
        set
        {
            Player.Instance.element = value; 
            UIManager.Instance.playerUI.UpdateElementUI(ElementType);
        }
    }
    [SerializeField] private float reactionRange;
    public float ReactionRange => reactionRange;

    [Header("攻击状态")]
    public bool isAttacking;
    public bool absorb;
    public bool release;
    public float clock = 0;
    [Header("能力解锁")]
    public bool lockFire = false;
    public bool lockElectricity = false;
    public bool lockBoom = false;
    public void LockFire(bool val)
    {
        lockFire = val;
        if (!val)
            Player.Instance.lockElement = false;
        else
            Player.Instance.lockElement = true;
    }
    public void UnlockElectricity() => lockElectricity = false;
    public void UnlockBoom() => lockBoom = false;

    [Header("目标信息")]
    public BaseObject target;
    public List<BaseObject> enemies;

    private Animator animator;
    private Camera mainCamera;
    
    public void OnAbsorb(InputValue value)
    {
        if (value.isPressed)
        {
            // 按下
            if (!isAttacking)
            {
                // 是主按键
                absorb = true;
                isAttacking = true;
                animator.SetBool("isAttack", true);
            }
            else
            {
                // 是释放技能
                var stage = !lockFire ? (!lockElectricity ? (!lockBoom ? 3 : 2) : 1) : 0;
                GetComponent<FollowerBehavier>().followerAttack.reactionStage = stage;
                if (target is IElement elementTarget)
                {
                    elementTarget.Reaction(this, stage);
                }
            }
        }
        else
        {
            // 松开
            if (absorb)
            {
                // 结束攻击
                isAttacking = false;
                absorb = false;
                clock = 0;
                animator.SetBool("isAttack", false);
            }
        }
    }
    public void OnRelease(InputValue value)
    {
        if (value.isPressed)
        {
            if (!isAttacking)
            {
                release = true;
                isAttacking = true;
                animator.SetBool("isAttack", true);
            }
            else
            {
                var stage = !lockFire ? (!lockElectricity ? (!lockBoom ? 3 : 2) : 1) : 0;
                GetComponent<FollowerBehavier>().followerAttack.reactionStage = stage;
                if (target is IElement elementTarget)
                {
                    elementTarget.Reaction(this, stage);
                }
            }
        }
        else
        {
            if (release)
            {
                isAttacking = false;
                release = false;
                clock = 0;
                animator.SetBool("isAttack", false);
            }
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (enemies.Count == 0)     
            target = null;
        Lock();
        DoAttack();
    }
    
    
    public void ReactionAttack(IElement t)
    {
        target.TakeDamage(damage * 2);
        Player.Instance.AddEP(3);
    }

    private void Lock()
    {
        if (mainCamera == null || enemies.Count == 0)
        {
            if (target) target.ShowBar(false);
            target = null;
            return;
        }
        
        // 从屏幕中心发射射线
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        
        // 存储最接近射线的敌人和距离
        BaseObject closestEnemy = null;
        float minDistance = Mathf.Infinity;
        
        // 隐藏所有敌人的血条
        foreach (var e in enemies)
        {
            e.ShowBar(false);
            
            // 检查敌人是否在相机视野内
            Vector3 screenPos = mainCamera.WorldToViewportPoint(e.transform.position);
            if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1 || screenPos.z < 0)
            {
                continue; // 敌人不在视野内
            }
            
            // 计算敌人到射线的距离
            float distanceToRay = GetDistanceToRay(ray, e.transform.position);
            
            // 选择最接近射线的敌人
            if (distanceToRay < minDistance)
            {
                minDistance = distanceToRay;
                closestEnemy = e;
            }
        }
        
        // 更新目标并显示血条
        target = closestEnemy;
        if (target)
        {
            target.ShowBar(true);
        }
    }
    
    // 计算点到射线的距离
    private float GetDistanceToRay(Ray ray, Vector3 point)
    {
        Vector3 closestPoint = ray.GetPoint(Vector3.Dot(point - ray.origin, ray.direction));
        return Vector3.Distance(point, closestPoint);
    }

    private void DoAttack()
    {
        if (!isAttacking || target is null) return;
        
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        clock += Time.deltaTime;
        
        var takeDamage = Damage;
        var takeVelocity = velocity;
        if (target is IElement elementTarget)
        {
            switch (elementTarget.ElementState)
            {
                case Element.fire:
                    takeDamage += 10;
                    break;
                case Element.electricity:
                    takeVelocity /= 2;
                    break;
            }
        }
        if (clock >= takeVelocity)
        {
            clock = 0;
            

            if (release && Player.Instance.EP > 0)
            {
                // TODO: 目标满能量时给予恢复的效果？？？
                if (target.currentEnergy == target.maxEnergy)   return;
                
                Player.Instance.MinusEP(1);
                target.Heal(Damage);
            }

            if (absorb)
            {
                if (target.currentEnergy == 0) return;

                Player.Instance.AddEP(1);
                target.TakeDamage(takeDamage);
            }
        }
    }
}