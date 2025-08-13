using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    static readonly string PathStr =
        System.IO.Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(SaveData data)
    {
        File.WriteAllText(PathStr, JsonUtility.ToJson(data));
#if UNITY_EDITOR
        Debug.Log("Saved: " + PathStr);
#endif
    }

    public static SaveData Load()
    {
        if (!File.Exists(PathStr)) return null;
        return JsonUtility.FromJson<SaveData>(File.ReadAllText(PathStr));
    }
}


