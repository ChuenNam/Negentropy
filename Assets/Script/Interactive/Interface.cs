using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanBeAttack
{
    public abstract void OnAttackedInvoke(Attacker attacker);
}

public interface IBreakable
{
    public void OnBreakInvoke(Attacker attacker);
}
