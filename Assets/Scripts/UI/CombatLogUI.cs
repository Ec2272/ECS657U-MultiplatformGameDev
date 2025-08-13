using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CombatLogUI : MonoBehaviour
{
    [Header("Wiring")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private TextMeshProUGUI logEntryTemplate;

    [Header("Behaviour")]
    [SerializeField] private int maxEntries = 100;
    [SerializeField] private bool showTimestamps = true;

    private readonly Queue<GameObject> entries = new();

    void Awake()
    {
        if (logEntryTemplate) logEntryTemplate.gameObject.SetActive(false);
    }

    public void AddHeader(string text) => AddLine($"<b>{text}</b>");
    public void AddSeparator() => AddLine("<color=#888>──────────────</color>");

    public void AddLine(string message)
    {
        if (!logEntryTemplate || !content) return;

        var go = Instantiate(logEntryTemplate.gameObject, content);
        go.SetActive(true);

        var tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.enableWordWrapping = true;
        tmp.text = showTimestamps
            ? $"<color=#999>[{System.DateTime.Now:HH:mm:ss}]</color> {message}"
            : message;

        entries.Enqueue(go);
        while (entries.Count > maxEntries)
        {
            var old = entries.Dequeue();
            if (old) Destroy(old);
        }

        // snap to bottom
        Canvas.ForceUpdateCanvases();
        if (scrollRect)
        {
            scrollRect.verticalNormalizedPosition = 0f;
            Canvas.ForceUpdateCanvases();
        }
    }
}

