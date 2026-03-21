using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("攻击状态")]
    public bool isAttacking;
    public bool absorb;
    public bool release;

    [Header("目标信息")]
    public Transform target;

    

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
    }
}
