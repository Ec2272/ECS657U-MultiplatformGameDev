using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameState : MonoBehaviour
{
    public static GameState I { get; private set; }
    private readonly HashSet<string> defeated = new();
    public string pendingEnemyId;
    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MarkDefeated(string id)
    {
        if (!string.IsNullOrEmpty(id)) defeated.Add(id);
    }

    public bool IsDefeated(string id) => !string.IsNullOrEmpty(id) && defeated.Contains(id);


    public List<string> ExportDefeated() => defeated.ToList();
    public void ImportDefeated(IEnumerable<string> ids)
    {
        defeated.Clear();
        if (ids == null) return;
        foreach (var id in ids) defeated.Add(id);
    }
}
