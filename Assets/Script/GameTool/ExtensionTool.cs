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
        Debug.Log(444);
        var startScale = transform.localScale;
        var elapsedTime = 0f;
        
        while (elapsedTime < transformTime)
        {
            if (transform != null)
                transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / transformTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (transform != null) transform.localScale = targetScale;
    }
    public static IEnumerator TransformShape(this Transform transform, TransformGroup targetTransform, float transformTime)
    {
        var startPosition = transform.localPosition;
        var startRotation = transform.localEulerAngles;
        var startScale = transform.localScale;

        // 根据枚举标志决定哪些属性需要变换
        var doPosition = (targetTransform.items & ShowTransformItem.Position) != 0;
        var doRotation = (targetTransform.items & ShowTransformItem.Rotation) != 0;
        var doScale    = (targetTransform.items & ShowTransformItem.Scale) != 0;

        var elapsedTime = 0f;
        while (elapsedTime < transformTime)
        {
            var t = elapsedTime / transformTime;
            if (doPosition)
                transform.localPosition = Vector3.Lerp(startPosition, targetTransform.Position, t);
            if (doRotation)
                transform.localEulerAngles = Vector3.Lerp(startRotation, targetTransform.Rotation, t);
            if (doScale)
                transform.localScale = Vector3.Lerp(startScale, targetTransform.Scale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 确保最终值正确（只应用需要变换的属性）
        if (doPosition) transform.localPosition = targetTransform.Position;
        if (doRotation) transform.localEulerAngles = targetTransform.Rotation;
        if (doScale) transform.localScale = targetTransform.Scale;
    }
}

[Flags]
public enum ShowTransformItem
{
    Position = 1 << 0,
    Rotation = 1 << 1,
    Scale = 1 << 2
}

[Serializable]
public class TransformGroup
{
    public ShowTransformItem items = ShowTransformItem.Position | ShowTransformItem.Rotation | ShowTransformItem.Scale;
    
    [ShowIf("items", ShowTransformItem.Position)]
    public Vector3 Position;
    
    [ShowIf("items", ShowTransformItem.Rotation)]
    public Vector3 Rotation;
    
    [ShowIf("items", ShowTransformItem.Scale)]
    public Vector3 Scale = Vector3.one;
}

[System.AttributeUsage(System.AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class ShowIfAttribute : PropertyAttribute
{
    public string PropertyName { get; private set; }
    public object CompareValue { get; private set; }

    public ShowIfAttribute(string propertyName, object compareValue)
    {
        PropertyName = propertyName;
        CompareValue = compareValue;
    }
}