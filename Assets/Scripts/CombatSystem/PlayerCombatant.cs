using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatant : CombatantBase
{

    [Header("Shared state")]
    public PlayerStatsSO stats;


    public readonly List<CombatAction> preparedActions = new();
    public override void ChooseActions()
    {
        
        Actions.Clear();
        Debug.Log("Player is choosing actions...");

        if (preparedActions.Count > 0)
        {
            Actions.AddRange(preparedActions);
            preparedActions.Clear();
        }
        
    }

    private void Awake()
    {
        // Set initial stats
        Name = "Player";
        Health = 100;
        Speed = 10;
        Heat = 0;
        Facing = CombatSystem.Enums.ZoneDirection.Front;
        Distance = CombatSystem.Enums.ZoneDistance.Mid;
        StepChoice = CombatSystem.Enums.StepType.None;
        Actions = new List<CombatAction>();
    }
    private void OnDisable()
    {
        if (stats != null){
            stats.currentHealth = Health; //save
            stats.currentHeat   = Heat;
        } 
    }
}


