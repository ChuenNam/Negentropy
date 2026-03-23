using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : BaseObject, ICanBeAttack, IBreakable
{
    [Header("基础配置")]
    public bool canBreak = false;
    public GameObject shieldPrefab;
    private GameObject shield;

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
        if (player == null)     return;
        var p = player.GetComponent<Attack>();
        p.target = null;
        p.enemies.Remove(this);
        
        Destroy(shield);
        Destroy(energyBarInstance);
    }

    public void OnAttackedInvoke(Attacker attacker)
    {
        if (canBreak && attacker.AttackType == AttackType.ball)
        {
            OnBreakInvoke(attacker);
        }
        
        TakeDamage(attacker.damage);
    }

    public void OnBreakInvoke(Attacker attacker)
    {
        GetComponent<BaseEnemy>().canInteract = true;
        this.enabled = false;
    }
    
    
}
