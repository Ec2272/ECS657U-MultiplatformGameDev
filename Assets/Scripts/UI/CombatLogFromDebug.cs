using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-5000)]//run before others
public class CombatLogFromDebug : MonoBehaviour
{
    [SerializeField] private CombatLogUI logUI;
    [SerializeField] private string prefix = "[Combat]";

    void OnEnable() => Application.logMessageReceived += HandleLog;
    void OnDisable() => Application.logMessageReceived -= HandleLog;

    void HandleLog(string condition, string stackTrace, LogType type)
    {
        if (type != LogType.Log) return;
        if (!condition.StartsWith(prefix)) return;

        string pretty = condition.Substring(prefix.Length).TrimStart();
        logUI?.AddLine(pretty);
    }
}

