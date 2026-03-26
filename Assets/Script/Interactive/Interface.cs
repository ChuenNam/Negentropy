using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanBeAttack
{
    public abstract void OnAttackedInvoke(Attacker attacker);       // 被攻击时效果的调用
    public Action OnHitCallback {get;set;}
}

public interface IAttack
{
    public int Damage { get; set; }
    public Element ElementType { get; set; }
    public float ReactionRange { get; }
    public void ReactionAttack(IElement t);       // 元素反应攻击函数
}

public interface IBreakable
{
    public void OnBreakInvoke(Attacker attacker);       // 破坏时效果的调用
}

public interface IElement
{
    public Element ElementState { get; set; }
    public void Reaction(IAttack attacker, int unlockStage);     // 基于只有火元素触发，若有多种元素触发扩展则添加参数触发元素类型
}

public interface ICheckPlayer
{
    public float CheckRange { get; }
}