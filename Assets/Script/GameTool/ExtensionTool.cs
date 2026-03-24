using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public static class ElementExtensions
{
    public static void SetElementColor(this Element element, Image img)
    {
        img.color = element switch
        {
            Element.common => Color.white,
            Element.fire => Color.red,
            Element.electricity => Color.yellow,
            _ => img.color
        };
    }
    public static void SetElementColor(this Element element, Material mat)
    {
        mat.color = element switch
        {
            Element.common => Color.white,
            Element.fire => Color.red,
            Element.electricity => Color.yellow,
            _ => mat.color
        };
    }
}

public static class TransformExtensions
{
    public static IEnumerator TransformShape(this Transform transform, Vector3 targetScale, float transformTime)
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
    }
}



