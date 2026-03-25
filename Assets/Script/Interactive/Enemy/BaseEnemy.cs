using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemy : BaseObject, ICanBeAttack, IElement
{
    [Header("基础配置")]
    public int enemyDamage = 10;
    public int killEnergy;
    public Action OnHitCallback { get; set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnEnergyEmpty += () =>
        {
            Player.Instance.AddEP(killEnergy);
            Destroy(gameObject);
        };
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        OnHitCallback = null;
    }

    public virtual void OnAttackedInvoke(Attacker attacker)
    {
        if (!canInteract) return;
        OnHitCallback?.Invoke();
        OnHurt(attacker);
    }

    public virtual void OnHurt(Attacker attacker)
    {
        TakeDamage(attacker.Damage);
    }

    public Element elementState;
    public Element ElementState
    { 
        get => elementState;
        set => elementState = value;
    }
    
    public void Reaction(IAttack attacker)
    {
        if (attacker.ElementType != Element.fire)   
            return;
        switch (ElementState)
        {
            case Element.common:    // 无元素附着
                ElementState = Element.fire;
                break;
            case Element.fire:  // 火 + 火 => 雷
                elementState = Element.electricity;
                break;
            case Element.electricity:   // 雷 + 火 => 爆炸
                elementState = Element.common;
                attacker.ReactionAttack(this);
                break;
        }
        attacker.ElementType = Element.common;
        ElementState.SetElementColor(energyBarInstance.GetComponent<Image>());
    }
}
