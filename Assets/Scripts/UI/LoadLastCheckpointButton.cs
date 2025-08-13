using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLastCheckpointButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private bool disableIfNoCheckpoint = true;

    void Reset() { button = GetComponent<Button>(); }

    void Awake()
    {
        if (!button) button = GetComponent<Button>();
        if (button) button.onClick.AddListener(OnClickLoadCheckpoint);
    }

    void OnEnable()
    {
        if (!button || !disableIfNoCheckpoint) return;
        
        button.interactable = SaveSystem.Load() != null;
    }

    void OnClickLoadCheckpoint()
    {
        var data = SaveSystem.Load();
        if (data == null)
        {
            Debug.LogWarning("No checkpoint found to load.");
            return;
        }
        if (GameState.I != null)
        GameState.I.ImportDefeated(data.defeatedEnemyIds);

        // Load the saved scene; PlayerLoader will position the player on Start
        SceneManager.LoadScene(data.sceneIndex, LoadSceneMode.Single);
    }
}