using System;
using System.Collections.Generic;
using UnityEngine;

public class GizmosControl : MonoBehaviour
{
    [Header("交互范围")]
    public bool actRange;
    public Color actRangeColor;
    [Range(0,1)]public float actRangeAlpha = .1f;
    public Material actRangeGizmosMaterial;
    private readonly Dictionary<BaseObject, GameObject> actRangeSpheres = new();

    [Header("敌人攻击范围")] 
    public bool attackRange;
    public Color attackRangeColor;
    [Range(0,1)]public float attackRangeAlpha = .1f;
    public Material attackRangeGizmosMaterial;
    private readonly Dictionary<Shooter, GameObject> attackRangeSpheres = new();
    
    [Header("敌人追踪范围")] 
    public bool trackRange;
    public Color trackRangeColor;
    [Range(0,1)]public float trackRangeAlpha = .1f;
    public Material trackRangeGizmosMaterial;
    private readonly Dictionary<Tracker, GameObject> trackRangeSpheres = new();

    public void SetBool(int index, bool value)
    {
        switch (index)
        {
            case 0: actRange = value; break;
            case 1: attackRange = value; break;
            case 2: trackRange = value; break;
        }
    }
    
    private void Update()
    {
        if (actRange) UpdateActRangeSpheres();
        else DestroySpheres(actRangeSpheres);

        if (attackRange) UpdateAttackRangeSpheres();
        else DestroySpheres(attackRangeSpheres);
        
        if (trackRange) UpdateTrackRangeSpheres();
        else DestroySpheres(trackRangeSpheres);

    }

    private void UpdateActRangeSpheres()
    {
        // 查找所有拥有BaseObject脚本的物体
        var baseObjects = FindObjectsOfType<BaseObject>();
        var currentObjects = new HashSet<BaseObject>(baseObjects);
        
        // 移除不再存在的BaseObject对应的球体
        var toRemove = new List<BaseObject>();
        foreach (var pair in actRangeSpheres)
        {
            if (!currentObjects.Contains(pair.Key) || pair.Key == null)
            {
                if (pair.Value != null)
                {
                    Destroy(pair.Value);
                }
                toRemove.Add(pair.Key);
            }
        }
        foreach (var key in toRemove)
        {
            actRangeSpheres.Remove(key);
        }
        
        // 为新的或需要更新的BaseObject创建/更新球体
        foreach (var baseObject in baseObjects)
        {
            if (baseObject == null) continue;
            
            if (!actRangeSpheres.ContainsKey(baseObject))
            {
                // 创建新球体
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.name = $"RangeSphere_{baseObject.name}";
                
                // 移除碰撞器
                var collider = sphere.GetComponent<Collider>();
                if (collider != null)
                    Destroy(collider);
                
                // 赋予材质
                var renderer = sphere.GetComponent<Renderer>();
                if (renderer != null && actRangeGizmosMaterial != null)
                {
                    renderer.material = actRangeGizmosMaterial;
                    renderer.material.SetColor($"_Color", actRangeColor);
                    renderer.material.SetFloat($"_Alpha", actRangeAlpha);
                }
                
                // 设置为不可选择
                sphere.hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable;
                actRangeSpheres[baseObject] = sphere;
            }
            
            // 更新球体材质、位置和大小
            var sphereObj = actRangeSpheres[baseObject];
            if (sphereObj != null)
            {
                var renderer = sphereObj.GetComponent<Renderer>();
                if (renderer != null && actRangeGizmosMaterial != null)
                {
                    renderer.material = actRangeGizmosMaterial;
                    renderer.material.SetColor($"_Color", actRangeColor);
                    renderer.material.SetFloat($"_Alpha", actRangeAlpha);
                }
                sphereObj.transform.position = baseObject.transform.position;
                sphereObj.transform.localScale = new Vector3(baseObject.range * 2, baseObject.range * 2, baseObject.range * 2);
            }
        }
    }
    private void UpdateAttackRangeSpheres()
    {
        // 查找所有拥有BaseObject脚本的物体
        var baseObjects = FindObjectsOfType<Shooter>();
        var currentObjects = new HashSet<Shooter>(baseObjects);
        
        // 移除不再存在的BaseObject对应的球体
        var toRemove = new List<Shooter>();
        foreach (var pair in attackRangeSpheres)
        {
            if (!currentObjects.Contains(pair.Key) || pair.Key == null)
            {
                if (pair.Value != null)
                {
                    Destroy(pair.Value);
                }
                toRemove.Add(pair.Key);
            }
        }
        foreach (var key in toRemove)
        {
            attackRangeSpheres.Remove(key);
        }
        
        // 为新的或需要更新的BaseObject创建/更新球体
        foreach (var baseObject in baseObjects)
        {
            if (baseObject == null) continue;
            
            if (!attackRangeSpheres.ContainsKey(baseObject))
            {
                // 创建新球体
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.name = $"RangeSphere_{baseObject.name}";
                
                // 移除碰撞器
                var collider = sphere.GetComponent<Collider>();
                if (collider != null)
                    Destroy(collider);
                
                // 赋予材质
                var renderer = sphere.GetComponent<Renderer>();
                if (renderer != null && attackRangeGizmosMaterial != null)
                {
                    renderer.material = attackRangeGizmosMaterial;
                    renderer.material.SetColor($"_Color", attackRangeColor);
                    renderer.material.SetFloat($"_Alpha", attackRangeAlpha);
                }
                
                // 设置为不可选择
                sphere.hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable;
                attackRangeSpheres[baseObject] = sphere;
            }
            
            // 更新球体材质、位置和大小
            var sphereObj = attackRangeSpheres[baseObject];
            if (sphereObj != null)
            {
                var renderer = sphereObj.GetComponent<Renderer>();
                if (renderer != null && attackRangeGizmosMaterial != null)
                {
                    renderer.material = attackRangeGizmosMaterial;
                    renderer.material.SetColor($"_Color", attackRangeColor);
                    renderer.material.SetFloat($"_Alpha", attackRangeAlpha);
                }
                sphereObj.transform.position = baseObject.transform.position;
                sphereObj.transform.localScale = new Vector3(baseObject.checkRange * 2, baseObject.checkRange * 2, baseObject.checkRange * 2);
            }
        }
    }
    private void UpdateTrackRangeSpheres()
    {
        // 查找所有拥有BaseObject脚本的物体
        var baseObjects = FindObjectsOfType<Tracker>();
        var currentObjects = new HashSet<Tracker>(baseObjects);
        
        // 移除不再存在的BaseObject对应的球体
        var toRemove = new List<Tracker>();
        foreach (var pair in trackRangeSpheres)
        {
            if (!currentObjects.Contains(pair.Key) || pair.Key == null)
            {
                if (pair.Value != null)
                {
                    Destroy(pair.Value);
                }
                toRemove.Add(pair.Key);
            }
        }
        foreach (var key in toRemove)
        {
            trackRangeSpheres.Remove(key);
        }
        
        // 为新的或需要更新的BaseObject创建/更新球体
        foreach (var baseObject in baseObjects)
        {
            if (baseObject == null) continue;
            
            if (!trackRangeSpheres.ContainsKey(baseObject))
            {
                // 创建新球体
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.name = $"RangeSphere_{baseObject.name}";
                
                // 移除碰撞器
                var collider = sphere.GetComponent<Collider>();
                if (collider != null)
                    Destroy(collider);
                
                // 赋予材质
                var renderer = sphere.GetComponent<Renderer>();
                if (renderer != null && trackRangeGizmosMaterial != null)
                {
                    renderer.material = trackRangeGizmosMaterial;
                    renderer.material.SetColor($"_Color", trackRangeColor);
                    renderer.material.SetFloat($"_Alpha", trackRangeAlpha);
                }
                
                // 设置为不可选择
                sphere.hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable;
                trackRangeSpheres[baseObject] = sphere;
            }
            
            // 更新球体材质、位置和大小
            var sphereObj = trackRangeSpheres[baseObject];
            if (sphereObj != null)
            {
                var renderer = sphereObj.GetComponent<Renderer>();
                if (renderer != null && trackRangeGizmosMaterial != null)
                {
                    renderer.material = trackRangeGizmosMaterial;
                    renderer.material.SetColor($"_Color", trackRangeColor);
                    renderer.material.SetFloat($"_Alpha", trackRangeAlpha);
                }
                sphereObj.transform.position = baseObject.transform.position;
                sphereObj.transform.localScale = new Vector3(baseObject.checkRange * 2, baseObject.checkRange * 2, baseObject.checkRange * 2);
            }
        }
    }


    private void DestroySpheres(Dictionary<BaseObject, GameObject> spheres)
    {
        foreach (var pair in spheres)
        {
            if (pair.Value != null)
                Destroy(pair.Value);
        }
        spheres.Clear();
    }
    private void DestroySpheres(Dictionary<Shooter, GameObject> spheres)
    {
        foreach (var pair in spheres)
        {
            if (pair.Value != null)
                Destroy(pair.Value);
        }
        spheres.Clear();
    }
    private void DestroySpheres(Dictionary<Tracker, GameObject> spheres)
    {
        foreach (var pair in spheres)
        {
            if (pair.Value != null)
                Destroy(pair.Value);
        }
        spheres.Clear();
    }
    private void OnDestroy()
    {
        DestroySpheres(actRangeSpheres);
        DestroySpheres(attackRangeSpheres);
        DestroySpheres(trackRangeSpheres);
    }
}