using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : BaseObject
{
    protected override void OnEnable()
    {
        base.OnEnable();

        OnEnergyChange += () =>
        {
            Player.Instance.SetEP(Player.Instance.MaxEP);
        };
    }
    
}
