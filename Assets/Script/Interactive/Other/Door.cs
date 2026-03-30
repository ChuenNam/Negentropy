using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ConditionObject
{
    [SerializeField]private BaseObject targetObj;
    [SerializeField]private int targetEnergy;
    public bool CheckCondition() => targetObj.currentEnergy == targetEnergy;
}


public class Door : MonoBehaviour
{
    public bool isLocked;
    public List<ConditionObject> conditionObjects = new();
    public UnityEvent unlockAction;

    private void Update()
    {
        if (unlockAction == null)   return;
        
        isLocked = false;
        foreach (var obj in conditionObjects)
        {
            if (obj.CheckCondition())   continue;
            isLocked = true;
            break;
        }
        if (!isLocked)
        {
            unlockAction?.Invoke();
            unlockAction = null;
        }
    }

    public void DestoryDoor()
    {
        Destroy(gameObject);
    }
    public void GenEnemy(GameObject e)
    {
        Instantiate(e, transform.position, Quaternion.identity);
    }
    
    private void Start()
    {
        if (TryGetComponent(out MeshRenderer mr) && mr.material.shader.name == "Universal Render Pipeline/Lit")
            mr.material.color = Color.yellow;
        
        if (unlockAction != null && conditionObjects.Count == 0)
        {
            deadAction = unlockAction;
        }
    }
    private UnityEvent deadAction;
    private void OnDestroy()
    {
        deadAction?.Invoke();
        deadAction = null;
    }
}
