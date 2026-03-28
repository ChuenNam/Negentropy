using System.Collections.Generic;
using UnityEngine;

public class Logs : MonoBehaviour
{
    public List<LogNode> logLink = new();

    public void PlayLogs()
    {
        UIManager.Instance.logUI.AddToLogLink(logLink);
    }
}
