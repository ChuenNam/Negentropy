using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    none,
    spike,
    ball,
}

public class Attacker : MonoBehaviour
{
    [SerializeField] private AttackType attackType;
    public AttackType AttackType
    {
        get => attackType;
        set => attackType = value;
    }
    
    public int damage;
    
    private void OnTriggerEnter(Collider other)
    {
        var list = other.GetComponents<ICanBeAttack>();
        foreach (var target in list)
        {
            var obj = target as BaseObject;
            if (obj.enabled && obj.canInteract)
            {
                target.OnAttackedInvoke(this);
                break;
            }
        }
        // 重置数据
        attackType = AttackType.none;
        damage = 0;
    }
}
