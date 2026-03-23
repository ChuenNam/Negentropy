using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : BaseObject, ICanBeAttack, IBreakable
{
    [Header("属性")]
    public bool canBreak = false;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        OnEnergyEmpty += () =>  canBreak = true;
    }

    public void OnAttackedInvoke(Attacker attacker)
    {
        if (!canInteract)    return;
        if (canBreak && attacker.AttackType == AttackType.ball)
        {
            Debug.Log("Break");
            OnBreakInvoke(attacker);
        }
    }

    public void OnBreakInvoke(Attacker attacker)
    {
        Destroy(gameObject);
    }
}
