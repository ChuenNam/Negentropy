using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseObject, ICanBeAttack
{
    [Header("基础配置")]
    public int killEnergy;

    protected override void OnEnable()
    {
        base.OnEnable();
        OnEnergyEmpty += () =>
        {
            Player.Instance.AddEP(killEnergy);
            Destroy(gameObject);
        };
    }

    public virtual void OnAttackedInvoke(Attacker attacker)
    {
        if (!canInteract) return;

        OnHurt(attacker);
    }

    public virtual void OnHurt(Attacker attacker)
    {
        TakeDamage(attacker.damage);
    }
}
