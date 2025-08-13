using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(Collider))]
public class EnemyOverworld : MonoBehaviour
{
    [SerializeField] private string enemyId;
    [SerializeField] private string battleSceneName = "BattleScene";
    [SerializeField] private string playerTag = "Player";

    void Awake()
    {
        // If beaten, don't spawn it again.
        if (GameState.I != null && GameState.I.IsDefeated(enemyId))
        {
            Destroy(gameObject);
        }
    }

    void OnValidate()
    {
        // Ensure unique ID per enemy instance)
        if (string.IsNullOrWhiteSpace(enemyId))
            enemyId = Guid.NewGuid().ToString();


        var col = GetComponent<Collider>();
        if (col && !col.isTrigger) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (GameState.I != null)
            GameState.I.pendingEnemyId = enemyId;

        SceneManager.LoadScene(battleSceneName, LoadSceneMode.Single);
    }
}

