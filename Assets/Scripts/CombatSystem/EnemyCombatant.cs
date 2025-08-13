using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem.Enums;

public class EnemyCombatant : CombatantBase
{
    //basic AI
    [Header("Enemy Move Pool")]
    public List<MoveDataSO> possibleMoves;

    public override void ChooseActions()
    {
        Actions.Clear();

        if (possibleMoves == null || possibleMoves.Count == 0)
        {
            Debug.LogWarning("Enemy has no moves assigned!");
            return;
        }

        // Pick 1 or 2 random moves
        int count = 2;
        for (int i = 0; i < count; i++)
        {
            var move = possibleMoves[Random.Range(0, possibleMoves.Count)];

            CombatAction action = move.actionType switch
            {
                ActionType.HalfAttack => new HalfAttackAction(move),
                ActionType.FullAttack => new FullAttackAction(move),
                ActionType.Dodge => new DodgeAction(move),
                ActionType.Block => new BlockAction(move),
                ActionType.Step => new StepAction(move),
                _ => null
            };

            if (action is StepAction)
                StepChoice = move.stepDirection;
            else
                Actions.Add(action);
        }

        Debug.Log("[Combat] " + "Enemy is choosing actions...");
    }



    private void Awake()
    {
        Name = "Enemy";
        Health = 50;
        Speed = 1;
        Heat = 0;
        Facing = CombatSystem.Enums.ZoneDirection.Front;
        Distance = CombatSystem.Enums.ZoneDistance.Mid;
        StepChoice = CombatSystem.Enums.StepType.None;
        Actions = new List<CombatAction>();
    }
}

