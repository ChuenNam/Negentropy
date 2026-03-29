using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : BaseObject, ICanBeAttack, IBreakable
{
    [Header("属性")]
    public bool canBreak = false;
    public int defaultEnergy = 0;
    public Action OnHitCallback { get; set; }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        currentEnergy = defaultEnergy;
        UpdateEnergyBar();
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
        OnHitCallback?.Invoke();
        if (currentEnergy == 0 && attacker.AttackType == AttackType.spike)
        {
            Player.Instance.AddEP(2);
            return;
        }
        TakeDamage(attacker.Damage);
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
