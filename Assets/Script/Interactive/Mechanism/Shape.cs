using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : BaseObject
{
    public int defaultEnergy;
    public float transformTime;
    public List<int> stage;
    public List<Vector3> stageTransform;

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
                if (i < stageTransform.Count)
                {
                    StartCoroutine(transform.TransformShape(stageTransform[i], transformTime));
                }
                break;
            }
        }
    }

    /*private IEnumerator TransformShape(Vector3 targetScale)
    {
        var startScale = transform.localScale;
        var elapsedTime = 0f;

        while (elapsedTime < transformTime)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / transformTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }*/
}