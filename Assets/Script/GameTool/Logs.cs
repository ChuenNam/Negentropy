using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Logs : MonoBehaviour
{
    public List<LogNode> logLink = new();
    public UnityEvent addedAction;

    public void PlayLogs()
    {
        UIManager.Instance.logUI.AddToLogLink(logLink);
        UIManager.Instance.logUI.addedAction = addedAction;
    }
}
