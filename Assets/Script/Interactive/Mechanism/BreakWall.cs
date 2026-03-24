using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : BaseObject, ICanBeAttack, IBreakable
{
    [Header("属性")]
    public bool canBreak = false;
    public Action OnHitCallback { get; set; }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        OnEnergyEmpty += () =>  canBreak = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnHitCallback = null;
    }

    public void OnAttackedInvoke(Attacker attacker)
    {
        if (!canInteract)    return;
        if (canBreak && attacker.AttackType == AttackType.ball)
        {
            Debug.Log("Break");
            OnHitCallback?.Invoke();
            OnBreakInvoke(attacker);
        }
    }

    public void OnBreakInvoke(Attacker attacker)
    {
        Destroy(gameObject);
    }
}
