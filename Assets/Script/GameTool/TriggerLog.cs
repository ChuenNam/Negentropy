using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LogNode
{
    public string logSpeaker;
    [TextArea] 
    public string logContent;
    public float waitTime = 1;
}

[RequireComponent(typeof(BoxCollider))]
public class TriggerLog : MonoBehaviour
{
    public bool isOnce;
    public List<LogNode> logLink = new();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) 
            return;
        UIManager.Instance.logUI.AddToLogLink(logLink);
        if (isOnce) 
            Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        var coll = GetComponent<BoxCollider>();
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, coll.size);
    }
}
