using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manifestation : BaseObject
{
    [Header("参数")] 
    public bool canRecycle;
    public int defaultEnergy;
    public Vector2 alphaRange;
    private Material material;
    private Collider coll;

    protected override void OnEnable()
    {
        base.OnEnable();
        currentEnergy = defaultEnergy;
        UpdateEnergyBar();
        
        material = GetComponent<Renderer>().material;
        material.SetFloat("_alpha", alphaRange.x);
        
        coll = GetComponent<Collider>();
        coll.enabled = false;

        OnEnergyFill += () =>
        {
            coll.enabled = true;
            material.SetColor("_Color", Color.gray);
            if (!canRecycle)
                HideEnergyBar();
        };
        OnEnergyChange += () =>
        {
            if (currentEnergy < maxEnergy)
            {
                coll.enabled = false;
                material.SetColor("_Color", Color.white);
            }
            
            var progress = (float)currentEnergy / maxEnergy;
            var alpha = Mathf.Lerp(alphaRange.x, alphaRange.y, progress);
                
            material.SetFloat("_Alpha", alpha);
        };
    }
}