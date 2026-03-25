using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : BaseObject
{
    public int defaultEnergy;
    public float transformTime;
    public List<int> stage;
    public List<TransformGroup> stageTransformGroup;

    protected override void OnEnable()
    {
        base.OnEnable();
        currentEnergy = defaultEnergy;
        UpdateEnergyBar();
        
        OnEnergyChange += ChangeStageShape;
    }

    private void ChangeStageShape()
    {
        for (var i = 0; i < stage.Count; i++)
        {
            if (stage[i] == currentEnergy)
            {
                if (i < stageTransformGroup.Count)
                {
                    StartCoroutine(transform.TransformShape(stageTransformGroup[i], transformTime));
                }
                break;
            }
        }
    }
}