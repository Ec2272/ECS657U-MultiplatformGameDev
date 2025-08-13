using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CombatSystem.Enums; // zones needed


public class OverworldCheckpointTrigger : MonoBehaviour
{
    [SerializeField] Transform playerRoot; // optional; will auto-find by tag

    void Awake()
    {
        if (playerRoot == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go) playerRoot = go.transform;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (playerRoot == null) playerRoot = other.transform;

        var p = playerRoot.position;
        var data = new SaveData
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex,
            x = p.x, y = p.y, z = p.z
        };
        if (GameState.I != null){
            data.defeatedEnemyIds = GameState.I.ExportDefeated();
        }
        
        SaveSystem.Save(data);
        
    }
}



