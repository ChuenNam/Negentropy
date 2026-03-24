using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : BaseObject, ICanBeAttack, IBreakable
{
    [Header("基础配置")]
    public bool canBreak = false;
    public GameObject shieldPrefab;
    private GameObject shield;
    
    public Action OnHitCallback { get; set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (shield == null)
        {
            shield = Instantiate(shieldPrefab, transform);
        }
        GetComponent<BaseEnemy>().canInteract = false;
        
        OnEnergyEmpty += () =>
        {
            canBreak = true;
        };
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnHitCallback = null;
        
        if (player == null)     return;
        var p = player.GetComponent<Attack>();
        p.target = null;
        p.enemies.Remove(this);
        
        Destroy(shield);
        Destroy(energyBarInstance);
    }

    public void OnAttackedInvoke(Attacker attacker)
    {
        OnHitCallback?.Invoke();
        TakeDamage(attacker.Damage);
        if (canBreak && attacker.AttackType == AttackType.ball)
        {
            OnBreakInvoke(attacker);
        }
    }

    public void OnBreakInvoke(Attacker attacker)
    {
        GetComponent<BaseEnemy>().canInteract = true;
        this.enabled = false;
    }
}
