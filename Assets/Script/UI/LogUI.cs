using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    public GameObject logPanel;
    public Text speaker;
    public Text content;
    
    public bool showing;
    private Queue<LogNode> logsQueue = new();
    public Action onLogOver;
    public UnityEvent addedAction;

    public void AddToLogLink(List<LogNode> logLink)
    {
        foreach (var logNode in logLink)
            logsQueue.Enqueue(logNode);
        if (!showing)
            ShowInfo();
    }
    
    public void ShowInfo()
    {
        logPanel.SetActive(true);
        showing = true;
        StartCoroutine(StartShowLog());
    }

    private IEnumerator StartShowLog()
    {
        var logs = logsQueue.ToArray();
        foreach (var logNode in logs)
        {
            speaker.text = logNode.logSpeaker;
            content.text = logNode.logContent;
            yield return new WaitForSeconds(logNode.waitTime);
            logsQueue.Dequeue();
        }
        if (logsQueue.Count != 0)
            yield return StartShowLog();
        
        addedAction?.Invoke();
        onLogOver?.Invoke();
        yield return null;
    }

    private void OnEnable()
    {
        onLogOver += () =>
        {
            showing = false;
            logPanel.SetActive(false);
        };
    }

    private void OnDisable()
    {
        onLogOver = null;
        addedAction = null;
    }
}
