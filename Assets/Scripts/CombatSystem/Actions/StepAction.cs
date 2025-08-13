using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem.Enums;

public class StepAction : CombatAction
{
    public StepAction(MoveDataSO moveInfo) : base(moveInfo) { }

    public override void Execute(CombatantBase user, CombatantBase target, int phase)
    {
        if (user.HasSteppedThisTurn)
        {
            Debug.LogWarning("[Combat] " + $"{user.Name} already stepped this turn and cannot step again.");
            return;
        }

        user.StepChoice = MoveInfo.stepDirection;
        user.HasSteppedThisTurn = true;

        Debug.Log("[Combat] " + $"{user.Name} will step {MoveInfo.stepDirection} this turn.");
    }

}
