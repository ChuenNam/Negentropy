using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("攻击参数")]
    public float velocity;
    public int damage = 10;
    
    [Header("攻击状态")]
    public bool isAttacking;
    public bool absorb;
    public bool release;
    public float clock = 0;

    [Header("目标信息")]
    public BaseObject target;
    public List<BaseObject> enemies;

    private Animator animator;
    
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
                Debug.Log("释放技能");
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
                Debug.Log("释放技能");
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
    }

    
    void Update()
    {
        Lock();
        
        DoAttack();
    }


    private void Lock()
    {
        var minDis = Mathf.Infinity;
        if (!enemies.Contains(target))
            target = null;
        foreach (var e in enemies)
        {
            var dis = Vector3.Distance(e.transform.position, transform.position);
            e.ShowBar(false);
            if (dis < minDis)
            {
                minDis = dis;
                target = e;
            }
        }
        if (target) target.ShowBar(true);
    }

    private void DoAttack()
    {
        if (!isAttacking || target is null) return;
        
        transform.LookAt(new Vector3(target.transform.position.x, 0, target.transform.position.z));
        clock += Time.deltaTime;
        if (clock >= velocity)
        {
            clock = 0;
            if (release)
            {
                target.Heal(damage);
                Player.Instance.MinusEP(1);
            }

            if (absorb)
            {
                target.TakeDamage(damage);
                Player.Instance.AddEP(1);
            }
        }
    }
    
}
