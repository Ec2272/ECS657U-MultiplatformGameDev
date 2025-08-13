using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CombatManagerMono : MonoBehaviour
{
    public PlayerCombatant player;
    public EnemyCombatant enemy;

    [Header("Scene flow")]
    [SerializeField] private string overworldSceneName = "SampleScene";
    [SerializeField] private float loadDelaySeconds = 1.0f;

    private CombatManager manager;

    private void Start()
    {
        Debug.Log("[Combat] --- Combat scene started ---");//sanity ping
        manager = new CombatManager(player, enemy);
        manager.OnCombatEnded += HandleCombatEnded;
        manager.StartTurn();
    }
    
    private void OnDestroy()
    {
        if (manager != null) manager.OnCombatEnded -= HandleCombatEnded;
    }
    
    private void HandleCombatEnded(CombatOutcome outcome)
    {
        Debug.Log($"[Combat] HandleCombatEnded fired: {outcome}");
        if (outcome == CombatOutcome.PlayerWon)
        {
            if (GameState.I != null){
                GameState.I.MarkDefeated(GameState.I.pendingEnemyId);
            }

            StartCoroutine(LoadOverworldAfterDelay());
        }
    }

    private IEnumerator LoadOverworldAfterDelay()
    {
        yield return new WaitForSeconds(loadDelaySeconds);
        SceneManager.LoadScene(overworldSceneName, LoadSceneMode.Single);
    }

    public void RunNextTurn()
    {
        manager.StartTurn();
    }
}
