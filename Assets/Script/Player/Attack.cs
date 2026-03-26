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
    public bool lockFire = false;
    public bool lockElectricity = false;
    public bool lockBoom = false;

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
    }
    void Update()
    {
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

            switch (absorb)
            {
                case true:
                {
                    if (target.currentEnergy == 0) return;
                
                    Player.Instance.AddEP(1);
                    target.TakeDamage(takeDamage);
                    break;
                }
            }
            
        }
    }
}
